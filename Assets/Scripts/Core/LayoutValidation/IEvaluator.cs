using UnityEngine;

public interface IEvaluator
{
    EvaluationMode Mode { get; }
    void Activate(ILayoutValidationProcessController processController);
    void Deactivate();
}
