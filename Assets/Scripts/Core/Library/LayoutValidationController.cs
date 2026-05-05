using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayoutValidationController : MonoBehaviour
{
    [SerializeField]
    private Button checkLayoutButton;
    [SerializeField]
    private GameObject validationMessagePrefab;
    [SerializeField]
    private Transform validationMessagesContainer;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material warningMaterial;
    [SerializeField]
    private Material errorMaterial;
    [SerializeField]
    private Material goodMaterial;

    private void OnEnable()
    {
        checkLayoutButton.onClick.AddListener(OnCheckLayoutClicked);
    }

    private void OnDisable()
    {
        checkLayoutButton.onClick.RemoveListener(OnCheckLayoutClicked);
    }

    private void OnCheckLayoutClicked()
    {
        int existingMessagesCount = validationMessagesContainer.childCount;

        for (int i = existingMessagesCount - 1; i >= 0; i--)
        {
            Destroy(validationMessagesContainer.GetChild(i).gameObject);
        }

        List<LibraryItem> errorItems = new List<LibraryItem>();
        List<LibraryItem> warningItems = new List<LibraryItem>();
        var items = FindObjectsByType<LibraryItem>(FindObjectsSortMode.None);
        bool anyWarnings = false;
        bool anyErrors = false;
        foreach (var item in items)
        {
            bool hasWarning = false;
            bool hasError = false;
            foreach (var port in item.Ports)
            {
                if (port.Type == PortType.Output)
                {
                    if (port.connectedPort != null)
                    {
                        if (!port.CompatibleItems.Contains(port.connectedPort.GetComponentInParent<LibraryItem>().ItemType) && !port.CompatibleCategories.Contains(port.connectedPort.GetComponentInParent<LibraryItem>().Category))
                        {
                            Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Error: {item.ItemName} port {port.PortName} cannot be connected to {port.connectedPort.GetComponentInParent<LibraryItem>().ItemName}";
                            errorItems.Add(item);
                            hasError = true;
                        }
                    }
                    else
                    {
                        Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Warning: {item.ItemName} port {port.PortName} is not connected to any port";
                        warningItems.Add(item);
                        hasWarning = true;
                    }
                }
            }

            if (hasError)
            {
                item.GetComponentInChildren<MeshRenderer>().sharedMaterial = errorMaterial;
                anyErrors = true;
            }
            else if (hasWarning)
            {
                item.GetComponentInChildren<MeshRenderer>().sharedMaterial = warningMaterial;
                anyWarnings = true;
            }
            else
            {
                item.GetComponentInChildren<MeshRenderer>().sharedMaterial = defaultMaterial;
            }
        }

        if (!anyErrors && !anyWarnings)
        {
            Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = "Validation successful";

            foreach (var item in items)
            {
                item.GetComponentInChildren<MeshRenderer>().sharedMaterial = goodMaterial;
            }
        }
    }
}