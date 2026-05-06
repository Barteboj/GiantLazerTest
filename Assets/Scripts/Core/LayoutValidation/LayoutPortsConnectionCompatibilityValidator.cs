using System.Collections.Generic;
using GiantLaserTest.Core.Library;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class LayoutPortsConnectionCompatibilityValidator : MonoBehaviour, ILayoutValidator
    {
        public ValidationResult Validate(LayoutState state)
        {
            var result = new ValidationResult();
            var elementResults = new List<ElementValidationResult>();

            foreach (var item in state.LibraryItems)
            {
                foreach (var port in item.Ports)
                {
                    if (port.Type == PortType.Output && port.connectedPort != null)
                    {
                        if (!port.CompatibleItems.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().ItemType) && !port.CompatibleCategories.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().Category))
                        {
                            elementResults.Add(new ElementValidationResult
                            {
                                RelatedItem = item,
                                ResultType = ElementValidationResultType.Error,
                                Message = $"Port {port.PortName} cannot be connected to {port.connectedPort.GetComponentInParent<ILibraryItem>().ItemName}"
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