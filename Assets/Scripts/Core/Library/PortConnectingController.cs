using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortConnectingController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference portClickAction;

    private Port currentPort;
    [SerializeField]
    private GameObject linePrefab;

    private void OnEnable()
    {
        portClickAction.action.performed += OnPortClicked;
    }

    private void OnDisable()
    {
        portClickAction.action.performed -= OnPortClicked;
    }

    private void OnPortClicked(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Port selectedPort = hit.collider.GetComponent<Port>();
            if (selectedPort != null && selectedPort.connectedPort == null)
            {
                if (currentPort != null && selectedPort.Type == PortType.Input)
                {
                    var currentLine = Instantiate(linePrefab).GetComponent<PortConnectionController>();
                    currentLine.Initialize(currentPort, selectedPort);
                    currentPort = null;
                }
                else if (selectedPort.Type == PortType.Output)
                {
                    currentPort = selectedPort;
                }
            }
            else
            {
                currentPort = null;
            }
        }
    }
}