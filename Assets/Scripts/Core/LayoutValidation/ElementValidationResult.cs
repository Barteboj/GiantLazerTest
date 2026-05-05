using UnityEngine;

public class ElementValidationResult
{
    public ElementValidationResultType ResultType { get; set; }
    public string Message { get; set; }
    public ILibraryItem RelatedItem { get; set; }
}