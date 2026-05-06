using System;
using System.Collections.Generic;
using System.Linq;
using GiantLaserTest.Attributes;
using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.LayoutValidation;
using GiantLaserTest.Core.Library;
using GiantLazerTest.App;
using TMPro;
using UnityEngine;

namespace GiantLaserTest.UI
{
    public class LayoutValidationPanelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Dropdown evaluationModeDropdown;
        [SerializeField]
        private GameObject validationMessagePrefab;
        [SerializeField]
        private Transform validationMessagesContainer;
        [SerializeField]
        private DeskController deskController;
        [SerializeField, RequireInterface(typeof(ILayoutValidationProcessController))]
        private UnityEngine.Object layoutValidationProcessControllerReference;

        [Header("Parameters")]
        [SerializeField, RequireInterface(typeof(IEvaluator))]
        private UnityEngine.Object[] layoutEvaluatorsReference;
        [SerializeField]
        private Material defaultMaterial;
        [SerializeField]
        private Material warningMaterial;
        [SerializeField]
        private Material errorMaterial;
        [SerializeField]
        private Material goodMaterial;

        private EvaluationMode[] evaluationModes;
        private ILayoutValidationProcessController layoutValidationProcessController;
        private IEvaluator[] layoutEvaluators;
        private IEvaluator activeEvaluator;

        private void Awake()
        {
            layoutValidationProcessController = layoutValidationProcessControllerReference as ILayoutValidationProcessController;
            layoutEvaluators = layoutEvaluatorsReference.OfType<IEvaluator>().ToArray();

            evaluationModes = Enum.GetValues(typeof(EvaluationMode)) as EvaluationMode[];

            foreach (EvaluationMode mode in evaluationModes)
            {
                evaluationModeDropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
            }

            evaluationModeDropdown.onValueChanged.AddListener(OnEvaluationModeChanged);
            layoutValidationProcessController.ValidationCompleted += OnValidationCompleted;
        }

        private void Start()
        {
            int modeIndex = Array.IndexOf(evaluationModes, GameController.Instance.StartingEvaluationMode);
            evaluationModeDropdown.value = modeIndex;
            LoadEvaluationMode(modeIndex);
        }

        private void OnEvaluationModeChanged(int evaluationModeIndex)
        {
            LoadEvaluationMode(evaluationModeIndex);
        }

        private void OnValidationCompleted(List<ValidationResult> validationResults)
        {
            DestroyExistingMessages();
            var libraryItems = deskController.LibraryItems;
            CreateMessages(validationResults, out var errorItems, out var warningItems);

            if (warningItems.Count == 0 && errorItems.Count == 0)
            {
                Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = "Validation successful";

                foreach (var item in libraryItems)
                {
                    item.Renderer.sharedMaterial = goodMaterial;
                }
            }
            else
            {
                foreach (var item in libraryItems)
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

        private void OnApplicationQuit()
        {
            evaluationModeDropdown.onValueChanged.RemoveListener(OnEvaluationModeChanged);
            layoutValidationProcessController.ValidationCompleted -= OnValidationCompleted;
            activeEvaluator?.Deactivate();
        }

        private void LoadEvaluationMode(int evaluationModeIndex)
        {
            activeEvaluator?.Deactivate();
            DestroyExistingMessages();
            var items = deskController.LibraryItems;

            foreach (var item in items)
            {
                item.Renderer.sharedMaterial = defaultMaterial;
            }

            var selectedMode = evaluationModes[evaluationModeIndex];
            var evaluatorToActivate = layoutEvaluators.First(x => x.Mode == selectedMode);
            evaluatorToActivate.Activate(layoutValidationProcessController);
            activeEvaluator = evaluatorToActivate;
        }

        private void DestroyExistingMessages()
        {
            int existingMessagesCount = validationMessagesContainer.childCount;

            for (int i = existingMessagesCount - 1; i >= 0; i--)
            {
                Destroy(validationMessagesContainer.GetChild(i).gameObject);
            }
        }

        private void CreateMessages(List<ValidationResult> validationResults, out List<ILibraryItem> itemsWithErrors, out List<ILibraryItem> itemsWithWarnings)
        {
            itemsWithErrors = new List<ILibraryItem>();
            itemsWithWarnings = new List<ILibraryItem>();

            foreach (var result in validationResults)
            {
                foreach (var elementResult in result.ElementResults)
                {
                    var item = elementResult.RelatedItem;
                    if (elementResult.ResultType == ElementValidationResultType.Error)
                    {
                        Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Error: {elementResult.Message}";
                        itemsWithErrors.Add(item);
                    }
                    else if (elementResult.ResultType == ElementValidationResultType.Warning)
                    {
                        Instantiate(validationMessagePrefab, validationMessagesContainer).GetComponentInChildren<TextMeshProUGUI>().text = $"Warning: {elementResult.Message}";
                        itemsWithWarnings.Add(item);
                    }
                }
            }
        }
    }
}