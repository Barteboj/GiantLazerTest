using System;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public interface ILayoutValidationProcessController
    {
        event Action<ValidationResult[]> OnValidationCompleted;
        void ValidateLayout();
    }
}