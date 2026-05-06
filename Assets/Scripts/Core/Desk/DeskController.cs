using System;
using System.Collections.Generic;
using UnityEngine;

public class DeskController : MonoBehaviour, IDeskController
{
    public event Action OnLibraryItemAdded;
    public event Action OnLibraryItemRemoved;

    [SerializeField]
    private float MaxSnappingDistance = 1f;
    [SerializeField]
    private LayerMask snapLayer;

    public List<ILibraryItem> LibraryItems { get; private set; } = new List<ILibraryItem>();

    private void OnEnable()
    {
        LibraryItemDraggingController.OnDraggingEnded += OnLibraryItemDraggingEnded;
        LibraryItem.OnLibraryItemDestroyed += OnLibraryItemDestroyed;
    }

    private void OnDisable()
    {
        LibraryItemDraggingController.OnDraggingEnded -= OnLibraryItemDraggingEnded;
        LibraryItem.OnLibraryItemDestroyed -= OnLibraryItemDestroyed;
    }

    private void OnLibraryItemDestroyed(LibraryItem item)
    {
        bool hasBeenOnDesk = LibraryItems.Remove(item);
        if (hasBeenOnDesk)
        {
            OnLibraryItemRemoved?.Invoke();
        }
    }

    private void OnLibraryItemDraggingEnded(LibraryItemDraggingController controller)
    {
        var libraryItem = controller.GetComponentInParent<LibraryItem>();
        if (Physics.Raycast(libraryItem.transform.position, -transform.up, out RaycastHit hit, MaxSnappingDistance, snapLayer))
        {
            libraryItem.transform.position = hit.point;
            RegisterLibraryItem(libraryItem);
        }
    }

    public void RegisterLibraryItem(LibraryItem item)
    {
        if (!LibraryItems.Contains(item))
        {
            item.GetComponentInChildren<LibraryItemDraggingController>().enabled = false;
            LibraryItems.Add(item);
            OnLibraryItemAdded?.Invoke();
        }
    }

    public void RemoveLibraryItem(LibraryItem item)
    {
        LibraryItems.Remove(item);
        Destroy(item.gameObject);
    }
}
