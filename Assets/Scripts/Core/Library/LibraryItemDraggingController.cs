using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GiantLaserTest.Core.Library
{
    public class LibraryItemDraggingController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static event Action<LibraryItemDraggingController> OnDraggingEnded;

        [Header("References")]
        [SerializeField]
        private Transform draggedTransform;

        private float zCoord;
        private Vector3 offset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            zCoord = Camera.main.WorldToScreenPoint(draggedTransform.position).z;
            Vector3 mouseWorldPosition = GetWorldPosition(eventData.position);
            offset = draggedTransform.position - mouseWorldPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            draggedTransform.position = GetWorldPosition(eventData.position) + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDraggingEnded?.Invoke(this);
        }

        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 screenPositionWithZ = new Vector3(screenPosition.x, screenPosition.y, zCoord);
            return Camera.main.ScreenToWorldPoint(screenPositionWithZ);
        }
    }
}