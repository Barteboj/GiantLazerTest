using System.Collections.Generic;
using GiantLaserTest.Core.Library;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GiantLaserTest.Core.Ports
{
    public class Port : MonoBehaviour, IPointerClickHandler
    {
        [Header("Parameters")]
        [field: SerializeField]
        public PortType Type { get; private set; }
        [field: SerializeField]
        public string PortName { get; private set; }
        [field: SerializeField]
        public List<LibraryCategory> CompatibleCategories { get; private set; }
        [field: SerializeField]
        public List<LibraryItemType> CompatibleItems { get; private set; }

        public Port connectedPort { get; private set; }

        private PortsConnectionController portConnectionController;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RemoveAttachedConnection();
            }
        }

        public void SetupConnection(PortsConnectionController portConnectionController, Port otherPort)
        {
            connectedPort = otherPort;
            this.portConnectionController = portConnectionController;
        }

        public void RemoveAttachedConnection()
        {
            if (connectedPort != null)
            {
                connectedPort.connectedPort = null;
                connectedPort = null;
                Destroy(portConnectionController.gameObject);
                portConnectionController = null;
            }
        }
    }
}