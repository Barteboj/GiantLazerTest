using System;
using System.Collections.Generic;
using GiantLaserTest.Core.Library;
using UnityEngine;

namespace GiantLaserTest.Core.Desk
{
    public class DeskController : MonoBehaviour
    {
        public event Action OnLibraryItemAdded;
        public event Action OnLibraryItemRemoved;

        [Header("Parameters")]
        [SerializeField]
        private float MaxSnappingDistance = 1f;
        [Header("References")]
        [SerializeField]
        private LayerMask snapLayer;
        [SerializeField]
        private LibraryItemsDraggingController libraryItemsDraggingController;

        public List<ILibraryItem> LibraryItems { get; private set; } = new List<ILibraryItem>();

        private void OnEnable()
        {
            libraryItemsDraggingController.DraggingEnded += OnLibraryItemDraggingEnded;
            LibraryItem.ItemDestroyed += OnLibraryItemDestroyed;
        }

        private void OnDisable()
        {
            libraryItemsDraggingController.DraggingEnded -= OnLibraryItemDraggingEnded;
            LibraryItem.ItemDestroyed -= OnLibraryItemDestroyed;
        }

        private void OnLibraryItemDraggingEnded(Transform draggedTransform)
        {
            var libraryItem = draggedTransform.GetComponent<LibraryItem>();

            if (Physics.Raycast(libraryItem.transform.position, -transform.up, out RaycastHit hit, MaxSnappingDistance, snapLayer))
            {
                libraryItem.transform.position = hit.point;
                RegisterLibraryItem(libraryItem);
            }
        }

        private void OnLibraryItemDestroyed(LibraryItem item)
        {
            bool hasBeenOnDesk = LibraryItems.Remove(item);
            if (hasBeenOnDesk)
            {
                OnLibraryItemRemoved?.Invoke();
            }
        }

        public void RegisterLibraryItem(LibraryItem item)
        {
            if (!LibraryItems.Contains(item))
            {
                LibraryItems.Add(item);
                item.LockInPlace();
                OnLibraryItemAdded?.Invoke();
            }
        }
    }
}