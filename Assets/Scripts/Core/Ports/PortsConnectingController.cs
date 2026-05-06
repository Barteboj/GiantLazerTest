using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.Library;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GiantLaserTest.Core.Ports
{
    public class PortsConnectingController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private DeskController deskController;
        [SerializeField]
        private GameObject portsConnectionPrefab;

        [Header("Parameters")]
        [SerializeField]
        private InputActionReference portClickInputAction;

        private Port currentPort;

        private void OnEnable()
        {
            portClickInputAction.action.performed += OnPortClickInputPerformed;
        }

        private void OnDisable()
        {
            portClickInputAction.action.performed -= OnPortClickInputPerformed;
        }

        private void OnPortClickInputPerformed(InputAction.CallbackContext context)
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Port selectedPort = hit.collider.GetComponent<Port>();
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
        }
    }
}