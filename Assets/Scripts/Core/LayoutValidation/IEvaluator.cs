namespace GiantLaserTest.Core.LayoutValidation
{
    public interface IEvaluator
    {
        EvaluationMode Mode { get; }
        void Activate(ILayoutValidationProcessController processController);
        void Deactivate();
    }
}