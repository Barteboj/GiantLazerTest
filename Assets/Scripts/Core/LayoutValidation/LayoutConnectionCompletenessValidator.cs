using System.Collections.Generic;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class LayoutConnectionCompletenessValidator : MonoBehaviour, ILayoutValidator
    {
        public ValidationResult Validate(LayoutState state)
        {
            var result = new ValidationResult();
            var elementResults = new List<ElementValidationResult>();

            foreach (var item in state.LibraryItems)
            {
                foreach (var port in item.Ports)
                {
                    if (port.Type == PortType.Output && port.connectedPort == null)
                    {
                        elementResults.Add(new ElementValidationResult
                        {
                            RelatedItem = item,
                            ResultType = ElementValidationResultType.Warning,
                            Message = $"{item.ItemName} port {port.PortName} is not connected to any port"
                        });
                    }
                }
            }

            result.ElementResults = elementResults.ToArray();
            return result;
        }
    }
}