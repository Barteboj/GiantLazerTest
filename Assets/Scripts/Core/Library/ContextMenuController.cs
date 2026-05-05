using System;
using System.Text;
using TMPro;
using UnityEngine;
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

    private LibraryItem contextItem;

    private void OnEnable()
    {
        editButton.onClick.AddListener(OnEditClicked);
        editCloseButton.onClick.AddListener(OnEditCloseClicked);
        deleteButton.onClick.AddListener(OnDeleteClicked);
    }

    private void OnDisable()
    {
        editButton.onClick.RemoveListener(OnEditClicked);
        editCloseButton.onClick.RemoveListener(OnEditCloseClicked);
        deleteButton.onClick.RemoveListener(OnDeleteClicked);
    }

    private void OnEditCloseClicked()
    {
        gameObject.SetActive(false);
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

        Destroy(contextItem.gameObject);
        gameObject.SetActive(false);
    }

    public void Activate(LibraryItem contextItem)
    {
        this.contextItem = contextItem;
        optionsPanel.SetActive(true);
        editPanel.SetActive(false);
        gameObject.SetActive(true);
    }
}
