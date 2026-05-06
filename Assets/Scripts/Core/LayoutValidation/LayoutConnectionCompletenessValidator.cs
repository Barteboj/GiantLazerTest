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

            foreach (var item in state.LibraryItems)
            {
                foreach (var port in item.Ports)
                {
                    if (port.Type == PortType.Output && port.connectedPort == null)
                    {
                        result.ElementResults.Add(new ElementValidationResult(ElementValidationResultType.Warning, $"{item.ItemName} port {port.PortName} is not connected to any port", item));
                    }
                }
            }

            return result;
        }
    }
}