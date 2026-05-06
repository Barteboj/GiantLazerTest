using System;
using UnityEngine;

public interface ILayoutValidationProcessController
{
    event Action<ValidationResult[]> OnValidationCompleted;
    void ValidateLayout();
}
