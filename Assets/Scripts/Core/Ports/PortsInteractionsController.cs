using System;
using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.Library;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GiantLaserTest.Core.Ports
{
    public class PortsInteractionsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private DeskController deskController;
        [SerializeField]
        private GameObject portsConnectionPrefab;

        [Header("Parameters")]
        [SerializeField]
        private InputActionReference createPortsConnectionInputAction;
        [SerializeField]
        private InputActionReference removePortsConnectionInputAction;

        private Port currentPort;

        private void OnEnable()
        {
            createPortsConnectionInputAction.action.performed += OnCreatePortsConnectionInputPerformed;
            removePortsConnectionInputAction.action.performed += OnRemovePortsConnectionInputPerformed;
        }

        private void OnDisable()
        {
            createPortsConnectionInputAction.action.performed -= OnCreatePortsConnectionInputPerformed;
            removePortsConnectionInputAction.action.performed -= OnRemovePortsConnectionInputPerformed;
        }

        private void OnCreatePortsConnectionInputPerformed(InputAction.CallbackContext context)
        {
            var selectedPort = GetPointedPort();

            if (selectedPort != null &&
                    selectedPort.connectedPort == null &&
                    deskController.LibraryItems.Contains(selectedPort.GetComponentInParent<ILibraryItem>()))
            {
                if (currentPort != null && selectedPort.Type == PortType.Input)
                {
                    var instantiatedPortsConnection = Instantiate(portsConnectionPrefab).GetComponent<PortsConnectionController>();
                    instantiatedPortsConnection.Initialize(currentPort, selectedPort);
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

        private void OnRemovePortsConnectionInputPerformed(InputAction.CallbackContext context)
        {
            var selectedPort = GetPointedPort();

            if (selectedPort != null)
            {
                selectedPort.RemoveAttachedConnection();
            }
        }

        private Port GetPointedPort()
        {
            Port selectedPort = null;
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectedPort = hit.collider.GetComponent<Port>();
            }

            return selectedPort;
        }
    }
}