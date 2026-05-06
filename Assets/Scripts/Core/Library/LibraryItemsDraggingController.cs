using System;
using GiantLaserTest.Core.Library;
using UnityEngine;
using UnityEngine.InputSystem;

public class LibraryItemsDraggingController : MonoBehaviour
{
    public event Action<Transform> DraggingEnded;

    [Header("Parameters")]
    [SerializeField]
    private InputActionReference dragAction;
    [SerializeField]
    private InputActionReference mousePositionAction;

    private bool isDragging = false;
    private float zDistance;
    private Vector3 offset;
    private Transform draggedTransform;

    private void OnEnable()
    {
        dragAction.action.started += OnDragInputActionStarted;
        dragAction.action.canceled += OnDragInputActionCanceled;
    }

    private void OnDisable()
    {
        dragAction.action.started -= OnDragInputActionStarted;
        dragAction.action.canceled -= OnDragInputActionCanceled;
    }

    private void Update()
    {
        if (isDragging)
        {
            Drag();
        }
    }

    private void OnDragInputActionStarted(InputAction.CallbackContext context)
    {
        Vector2 mousePos = mousePositionAction.action.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var libraryItem = hit.transform.parent?.GetComponent<ILibraryItem>();

            if (libraryItem != null && !libraryItem.IsLockedInPlace)
            {
                isDragging = true;
                zDistance = hit.distance;
                draggedTransform = libraryItem.GameObject.transform;

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zDistance));
                offset = draggedTransform.position - worldPos;
            }
        }
    }

    private void OnDragInputActionCanceled(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            isDragging = false;
            DraggingEnded?.Invoke(draggedTransform);
        }
    }

    private void Drag()
    {
        Vector2 currentMouseScreenPosition = mousePositionAction.action.ReadValue<Vector2>();
        Vector3 currentMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(currentMouseScreenPosition.x, currentMouseScreenPosition.y, zDistance));

        draggedTransform.position = currentMouseWorldPosition + offset;
    }
}
