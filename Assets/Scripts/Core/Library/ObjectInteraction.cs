using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    private Transform draggedTransform;

    private Camera mainCamera;
    private Vector3 offset;
    private float zCoord;

    void Awake() => mainCamera = Camera.main;

    // 1. Wywoływane w momencie rozpoczęcia przeciągania
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Zapamiętujemy głębię obiektu względem kamery
        zCoord = mainCamera.WorldToScreenPoint(draggedTransform.position).z;

        // Obliczamy różnicę między środkiem obiektu a punktem kliknięcia
        Vector3 mouseWorldPos = GetMouseWorldPos(eventData.position);
        offset = draggedTransform.position - mouseWorldPos;
    }

    // 2. Wywoływane co klatkę podczas ruchu myszy
    public void OnDrag(PointerEventData eventData)
    {
        // Dodajemy offset, aby obiekt nie "skakał" do kursora
        draggedTransform.position = GetMouseWorldPos(eventData.position) + offset;




        // Vector3 screenPos = eventData.position;
        // screenPos.z = mainCamera.WorldToScreenPoint(draggedTransform.position).z;
        
        // draggedTransform.position = mainCamera.ScreenToWorldPoint(screenPos);
    }

    private Vector3 GetMouseWorldPos(Vector2 screenPosition)
    {
        Vector3 screenPosWithZ = new Vector3(screenPosition.x, screenPosition.y, zCoord);
        return mainCamera.ScreenToWorldPoint(screenPosWithZ);
    }

    // Wykrywanie prawego kliknięcia zostaje bez zmian
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var contextMenu = Resources.FindObjectsOfTypeAll<ContextMenuController>()[0];
            contextMenu.Activate(GetComponentInParent<LibraryItem>());
            contextMenu.transform.position = eventData.position;
        }
    }
}
