using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LibraryItemDraggingController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static event Action<LibraryItemDraggingController> OnDraggingEnded;

    [SerializeField]
    private Transform draggedTransform;

    private Camera mainCamera;
    private Vector3 offset;
    private float zCoord;

    void Awake() => mainCamera = Camera.main;

    public void OnBeginDrag(PointerEventData eventData)
    {
        zCoord = mainCamera.WorldToScreenPoint(draggedTransform.position).z;

        Vector3 mouseWorldPos = GetMouseWorldPos(eventData.position);
        offset = draggedTransform.position - mouseWorldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggedTransform.position = GetMouseWorldPos(eventData.position) + offset;
    }

    private Vector3 GetMouseWorldPos(Vector2 screenPosition)
    {
        Vector3 screenPosWithZ = new Vector3(screenPosition.x, screenPosition.y, zCoord);
        return mainCamera.ScreenToWorldPoint(screenPosWithZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDraggingEnded?.Invoke(this);
    }
}
