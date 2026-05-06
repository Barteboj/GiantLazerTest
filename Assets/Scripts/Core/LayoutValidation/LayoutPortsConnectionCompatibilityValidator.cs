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

            foreach (var item in state.LibraryItems)
            {
                foreach (var port in item.Ports)
                {
                    if (port.Type == PortType.Output && port.connectedPort != null)
                    {
                        if (!port.CompatibleItems.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().ItemType) && !port.CompatibleCategories.Contains(port.connectedPort.GetComponentInParent<ILibraryItem>().Category))
                        {
                            ILibraryItem connectedItem = port.connectedPort.GetComponentInParent<ILibraryItem>();
                            result.ElementResults.Add(new ElementValidationResult(ElementValidationResultType.Error, $"{item.ItemName} Port {port.PortName} cannot be connected to {connectedItem.ItemName}", item));
                        }
                    }
                }
            }

            return result;
        }
    }
}