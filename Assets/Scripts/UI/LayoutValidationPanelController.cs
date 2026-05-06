using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LayoutValidationPanelController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown evaluationModeDropdown;
    [SerializeField]
    private GameObject validationMessagePrefab;
    [SerializeField]
    private Transform validationMessagesContainer;
    [SerializeField, RequireInterface(typeof(ILayoutValidator))]
    private UnityEngine.Object[] layoutValidatorsReference;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material warningMaterial;
    [SerializeField]
    private Material errorMaterial;
    [SerializeField]
    private Material goodMaterial;
    private ILayoutValidator[] layoutValidators;
    private EvaluationMode[] evaluationModes;
    [SerializeField, RequireInterface(typeof(ILayoutValidationProcessController))]
    private UnityEngine.Object layoutValidationProcessControllerReference;
    [SerializeField, RequireInterface(typeof(IEvaluator))]
    private UnityEngine.Object[] layoutEvaluatorsReference;
    [SerializeField]
    private DeskController deskController;

    private ILayoutValidationProcessController layoutValidationProcessController;
    private IEvaluator[] layoutEvaluators;
    private IEvaluator activeEvaluator;

    private void Awake()
    {
        layoutValidationProcessController = layoutValidationProcessControllerReference as ILayoutValidationProcessController;
        layoutValidators = layoutValidatorsReference.OfType<ILayoutValidator>().ToArray();
        layoutEvaluators = layoutEvaluatorsReference.OfType<IEvaluator>().ToArray();
        evaluationModes = Enum.GetValues(typeof(EvaluationMode)) as EvaluationMode[];
        foreach (EvaluationMode mode in evaluationModes)
        {
            evaluationModeDropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
        }
        evaluationModeDropdown.onValueChanged.AddListener(OnEvaluationModeChanged);
        layoutValidationProcessController.OnValidationCompleted += OnValidationCompleted;
    }

    private void Start()
    {
        int modeIndex = Array.IndexOf(evaluationModes, GameController.Instance.StartingEvaluationMode);
        evaluationModeDropdown.value = modeIndex;
        LoadEvaluationMode(modeIndex);
    }

    private void OnEvaluationModeChanged(int arg0)
    {
        LoadEvaluationMode(arg0);
    }

    private void LoadEvaluationMode(int value)
    {
        activeEvaluator?.Deactivate();
        DestroyCurrentMessages();
        var items = deskController.LibraryItems;

        foreach (var item in items)
        {
            item.Renderer.sharedMaterial = defaultMaterial;
        }

        var selectedMode = evaluationModes[value];
        var evaluatorToActivate = layoutEvaluators.First(x => x.Mode == selectedMode);
        evaluatorToActivate.Activate(layoutValidationProcessController);
        activeEvaluator = evaluatorToActivate;
    }

    private void OnApplicationQuit()
    {
        layoutValidationProcessController.OnValidationCompleted -= OnValidationCompleted;
        activeEvaluator?.Deactivate();
    }

    private void OnValidationCompleted(ValidationResult[] validationResults)
    {
        DestroyCurrentMessages();
        var items = deskController.LibraryItems;
        List<ILibraryItem> errorItems = new List<ILibraryItem>();
        List<ILibraryItem> warningItems = new List<ILibraryItem>();
        bool anyWarnings = false;
        bool anyErrors = false;
        foreach (var result in validationResults)
        {
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
    }

    private void DestroyCurrentMessages()
    {
        int existingMessagesCount = validationMessagesContainer.childCount;

        for (int i = existingMessagesCount - 1; i >= 0; i--)
        {
            Destroy(validationMessagesContainer.GetChild(i).gameObject);
        }
    }
}