using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LayoutValidationPanelController : MonoBehaviour
{
    [SerializeField]
    private Button checkLayoutButton;
    [SerializeField]
    private GameObject validationMessagePrefab;
    [SerializeField]
    private Transform validationMessagesContainer;
    [SerializeField, RequireInterface(typeof(ILayoutValidator))]
    private Object[] layoutValidatorsReference;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material warningMaterial;
    [SerializeField]
    private Material errorMaterial;
    [SerializeField]
    private Material goodMaterial;
    private ILayoutValidator[] layoutValidators;

    private void Awake()
    {
        layoutValidators = layoutValidatorsReference.OfType<ILayoutValidator>().ToArray();
    }

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

        List<ILibraryItem> errorItems = new List<ILibraryItem>();
        List<ILibraryItem> warningItems = new List<ILibraryItem>();
        var items = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<ILibraryItem>(true));
        bool anyWarnings = false;
        bool anyErrors = false;
        List<ValidationResult> validationResults = new List<ValidationResult>();
        LayoutState layoutState = new LayoutState { LibraryItems = items.ToArray() };
        foreach (var validator in layoutValidators)
        {
            var result = validator.Validate(layoutState);
            validationResults.Add(result);
            foreach (var elementResult in result.ElementResults)
            {
                var item = elementResult.RelatedItem;
                if (elementResult.ResultType == ElementValidationResultType.Error)
                {
                    Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Error: {elementResult.Message}";
                    errorItems.Add(item);
                    anyErrors = true;
                }
                else if (elementResult.ResultType == ElementValidationResultType.Warning)
                {
                    Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Warning: {elementResult.Message}";
                    warningItems.Add(item);
                    anyWarnings = true;
                }
            }
        }

        if (!anyWarnings && !anyErrors)
        {
            Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = "Validation successful";

            foreach (var item in items)
            {
                item.Renderer.sharedMaterial = goodMaterial;
            }
        }
        else
        {
            foreach (var item in items)
            {
                if (errorItems.Contains(item))
                {
                    item.Renderer.sharedMaterial = errorMaterial;
                }
                else if (warningItems.Contains(item))
                {
                    item.Renderer.sharedMaterial = warningMaterial;
                }
                else
                {
                    item.Renderer.sharedMaterial = defaultMaterial;
                }
            }
        }


        // foreach (var item in items)
        // {
        //     bool hasWarning = false;
        //     bool hasError = false;
        //     foreach (var port in item.Ports)
        //     {
        //         if (port.Type == PortType.Output)
        //         {
        //             if (port.connectedPort != null)
        //             {
        //                 if (!port.CompatibleItems.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().ItemType) && !port.CompatibleCategories.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().Category))
        //                 {
        //                     Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Error: {item.ItemName} port {port.PortName} cannot be connected to {port.connectedPort.GetComponentInParent<LibraryItem>().ItemName}";
        //                     errorItems.Add(item);
        //                     hasError = true;
        //                 }
        //             }
        //             else
        //             {
        //                 Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Warning: {item.ItemName} port {port.PortName} is not connected to any port";
        //                 warningItems.Add(item);
        //                 hasWarning = true;
        //             }
        //         }
        //     }

        //     if (hasError)
        //     {
        //         item.Renderer.sharedMaterial = errorMaterial;
        //         anyErrors = true;
        //     }
        //     else if (hasWarning)
        //     {
        //         item.Renderer.sharedMaterial = warningMaterial;
        //         anyWarnings = true;
        //     }
        //     else
        //     {
        //         item.Renderer.sharedMaterial = defaultMaterial;
        //     }
        // }

        // if (!anyErrors && !anyWarnings)
        // {
        //     Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = "Validation successful";

        //     foreach (var item in items)
        //     {
        //         item.Renderer.sharedMaterial = goodMaterial;
        //     }
        // }
    }
}