using GiantLaserTest.Core.Library;

namespace GiantLaserTest.Core.LayoutValidation
{
    public struct ElementValidationResult
    {
        public ElementValidationResultType ResultType { get; private set; }
        public string Message { get; private set; }
        public ILibraryItem RelatedItem { get; private set; }

        public ElementValidationResult(ElementValidationResultType resultType, string message, ILibraryItem relatedItem)
        {
            ResultType = resultType;
            Message = message;
            RelatedItem = relatedItem;
        }
    }
}