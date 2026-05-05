using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Port : MonoBehaviour, IPointerClickHandler
{
    public static event Action<Port> OnPortClicked;

    [field: SerializeField]
    public PortType Type { get; private set; }
    [field: SerializeField]
    public string PortName { get; private set; }
    [field: SerializeField]
    public List<LibraryCategory> CompatibleCategories { get; private set; }
    [field: SerializeField]
    public List<LibraryItemType> CompatibleItems { get; private set; }
    [field: SerializeField]
    public Port connectedPort { get; private set; }

    private PortConnectionController partConnectionController;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveAttachedConnection();
        }
    }

    public void SetupConnection(PortConnectionController portConnectionController, Port otherPort)
    {
        connectedPort = otherPort;
        this.partConnectionController = portConnectionController;
    }

    public void RemoveAttachedConnection()
    {
        if (connectedPort != null)
        {
            connectedPort.connectedPort = null;
            connectedPort = null;
            Destroy(partConnectionController.gameObject);
            partConnectionController = null;
        }
    }
}