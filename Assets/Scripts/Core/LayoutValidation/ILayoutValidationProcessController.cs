using System;
using System.Collections.Generic;

namespace GiantLaserTest.Core.LayoutValidation
{
    public interface ILayoutValidationProcessController
    {
        event Action<List<ValidationResult>> ValidationCompleted;
        void ValidateLayout();
    }
}