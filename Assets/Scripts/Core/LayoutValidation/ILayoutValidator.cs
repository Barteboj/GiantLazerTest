namespace GiantLaserTest.Core.LayoutValidation
{
    public interface ILayoutValidator
    {
        ValidationResult Validate(LayoutState state);
    }
}