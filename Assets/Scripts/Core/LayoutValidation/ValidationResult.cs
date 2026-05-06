using System.Collections.Generic;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class ValidationResult
    {
        public List<ElementValidationResult> ElementResults { get; private set; } = new List<ElementValidationResult>();
    }
}