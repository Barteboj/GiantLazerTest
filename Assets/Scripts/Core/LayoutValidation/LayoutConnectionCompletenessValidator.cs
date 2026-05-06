using System.Collections.Generic;
using UnityEngine;

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
