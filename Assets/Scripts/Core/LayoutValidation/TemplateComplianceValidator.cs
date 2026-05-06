using System.Collections.Generic;
using GiantLaserTest.Core.Library;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class TemplateComplianceValidator : MonoBehaviour, ILayoutValidator
    {
        [Header("References")]
        [SerializeField]
        private LayoutTemplateSO template;

        public ValidationResult Validate(LayoutState state)
        {
            var result = new ValidationResult();
            CheckMissingItems(state, result);
            CheckRedundantItems(state, result);
            CheckWrongConnections(state, result);
            return result;
        }

        private void CheckMissingItems(LayoutState state, ValidationResult result)
        {
            foreach (var templateElement in template.TemplateElements)
            {
                var matchingItem = state.LibraryItems.Find(x => x.ItemType == templateElement.LibraryItemType);

                if (matchingItem == null)
                {
                    result.ElementResults.Add(new ElementValidationResult(ElementValidationResultType.Warning, $"Missing required item of type {templateElement.LibraryItemType}", null));
                }
            }
        }

        private void CheckRedundantItems(LayoutState state, ValidationResult result)
        {
            foreach (var item in state.LibraryItems)
            {
                var templateElement = template.TemplateElements.Find(x => x.LibraryItemType == item.ItemType);

                if (templateElement == null)
                {
                    result.ElementResults.Add(new ElementValidationResult(ElementValidationResultType.Warning, $"Redundant item of type {item.ItemType}", item));
                }
            }
        }

        private void CheckWrongConnections(LayoutState state, ValidationResult result)
        {
            foreach (var templateElement in template.TemplateElements)
            {
                var matchingItem = state.LibraryItems.Find(x => x.ItemType == templateElement.LibraryItemType);

                if (matchingItem != null)
                {
                    foreach (var outputConnectedItem in templateElement.OutputPortsConnectedItems)
                    {
                        bool isExisting = matchingItem.Ports.Exists(p => p.Type == PortType.Output && p.connectedPort != null && p.connectedPort.GetComponentInParent<LibraryItem>().ItemType == outputConnectedItem);

                        if (!isExisting)
                        {
                            result.ElementResults.Add(new ElementValidationResult(ElementValidationResultType.Warning, $"{matchingItem.ItemName} output is not connected to {outputConnectedItem}", matchingItem));
                        }
                    }
                }
            }
        }
    }
}