using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayoutValidationProcessController : MonoBehaviour, ILayoutValidationProcessController
{
    public event Action<ValidationResult[]> OnValidationCompleted;

    [SerializeField, RequireInterface(typeof(ILayoutValidator))]
    private UnityEngine.Object[] layoutValidatorsReference;
    [SerializeField]
    private DeskController deskController;

    private ILayoutValidator[] layoutValidators;

    private void Awake()
    {
        layoutValidators = layoutValidatorsReference.OfType<ILayoutValidator>().ToArray();
    }

    public void ValidateLayout()
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        LayoutState layoutState = new LayoutState { LibraryItems = deskController.LibraryItems.ToArray() };

        foreach (var validator in layoutValidators)
        {
            var result = validator.Validate(layoutState);
            validationResults.Add(result);
        }

        OnValidationCompleted?.Invoke(validationResults.ToArray());
    }
}
