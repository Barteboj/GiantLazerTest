using GiantLaserTest.Core.Library;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class ElementValidationResult
    {
        public ElementValidationResultType ResultType { get; set; }
        public string Message { get; set; }
        public ILibraryItem RelatedItem { get; set; }
    }
}