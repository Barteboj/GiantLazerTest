using System;
using System.Collections.Generic;
using GiantLaserTest.Core.Library;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class TemplateComplianceValidator : MonoBehaviour, ILayoutValidator
    {
        [SerializeField]
        private LayoutTemplateSO template;

        public ValidationResult Validate(LayoutState state)
        {
            var result = new ValidationResult();
            var elementResults = new List<ElementValidationResult>();

            foreach (var templateElement in template.TemplateElements)
            {
                var matchingItem = Array.Find(state.LibraryItems, item => item.ItemType == templateElement.LibraryItemType);
                if (matchingItem == null)
                {
                    elementResults.Add(new ElementValidationResult
                    {
                        RelatedItem = null,
                        ResultType = ElementValidationResultType.Warning,
                        Message = $"Missing required item of type {templateElement.LibraryItemType}"
                    });
                }
            }

            foreach (var item in state.LibraryItems)
            {
                var templateElement = Array.Find(template.TemplateElements, x => x.LibraryItemType == item.ItemType);
                if (templateElement == null)
                {
                    elementResults.Add(new ElementValidationResult
                    {
                        RelatedItem = item,
                        ResultType = ElementValidationResultType.Warning,
                        Message = $"Redundant item of type {item.ItemType}"
                    });
                }
            }

            foreach (var templateElement in template.TemplateElements)
            {
                var matchingItem = Array.Find(state.LibraryItems, item => item.ItemType == templateElement.LibraryItemType);
                if (matchingItem != null)
                {
                    foreach (var outputConnectedItem in templateElement.OutputPortsConnectedItems)
                    {
                        bool isExisting = Array.Exists(matchingItem.Ports, p => p.Type == PortType.Output && p.connectedPort != null && p.connectedPort.GetComponentInParent<LibraryItem>().ItemType == outputConnectedItem);
                        if (!isExisting)
                        {
                            elementResults.Add(new ElementValidationResult
                            {
                                RelatedItem = matchingItem,
                                ResultType = ElementValidationResultType.Warning,
                                Message = $"{matchingItem.ItemName} output is not connected to {outputConnectedItem}"
                            });
                        }
                    }
                }
            }

            result.ElementResults = elementResults.ToArray();
            return result;
        }
    }
}