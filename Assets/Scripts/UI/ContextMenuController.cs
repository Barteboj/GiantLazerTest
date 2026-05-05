using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContextMenuController : MonoBehaviour
{
    [SerializeField]
    private Button editButton;
    [SerializeField]
    private Button deleteButton;
    [SerializeField]
    private Button editCloseButton;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject editPanel;
    [SerializeField]
    private TextMeshProUGUI editDescription;
    [SerializeField]
    private InputActionReference openInputAction;

    private ILibraryItem contextItem;

    private void OnEnable()
    {
        openInputAction.action.performed += OnOpenInputActionPerformed;
        editButton.onClick.AddListener(OnEditClicked);
        editCloseButton.onClick.AddListener(OnEditCloseClicked);
        deleteButton.onClick.AddListener(OnDeleteClicked);
    }

    private void OnDisable()
    {
        openInputAction.action.performed -= OnOpenInputActionPerformed;
        editButton.onClick.RemoveListener(OnEditClicked);
        editCloseButton.onClick.RemoveListener(OnEditCloseClicked);
        deleteButton.onClick.RemoveListener(OnDeleteClicked);
    }

    private void OnOpenInputActionPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.GetComponent<LibraryItemDraggingController>() != null)
            {
                ILibraryItem selectedItem = hit.collider.GetComponentInParent<ILibraryItem>();

                if (selectedItem != null)
                {
                    Activate(selectedItem);
                    transform.position = screenPos;
                }
            }
        }
    }

    private void OnEditCloseClicked()
    {
        editPanel.SetActive(false);
    }

    private void OnEditClicked()
    {
        optionsPanel.SetActive(false);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Name: {contextItem.ItemName}");
        stringBuilder.AppendLine($"Ports status:");
        foreach (var port in contextItem.Ports)
        {
            stringBuilder.AppendLine($"{port.PortName} {(port.Type == PortType.Output ? "->" : "<-")} {port.connectedPort?.PortName ?? "None"}");
        }

        editDescription.text = stringBuilder.ToString();
        editPanel.SetActive(true);
    }

    private void OnDeleteClicked()
    {
        foreach (var port in contextItem.Ports)
        {
            port.RemoveAttachedConnection();
        }

        Destroy(contextItem.GameObject);
        optionsPanel.SetActive(false);
    }

    public void Activate(ILibraryItem contextItem)
    {
        this.contextItem = contextItem;
        optionsPanel.SetActive(true);
        editPanel.SetActive(false);
    }
}
