using System.Collections;
using UnityEngine;

public class LearningEvaluator : MonoBehaviour, IEvaluator
{
    public EvaluationMode Mode => EvaluationMode.Learning;

    private ILayoutValidationProcessController processController;

    public void Activate(ILayoutValidationProcessController processController)
    {
        this.processController = processController;

        LibraryItem.OnLibraryItemCreated += OnLayoutChanged;
        LibraryItem.OnLibraryItemDestroyed += OnLayoutChanged;
        PortConnectionController.OnConnectionCreated += OnLayoutChanged;
        PortConnectionController.OnConnectionDestroyed += OnLayoutChanged;

        processController.ValidateLayout();
    }

    public void Deactivate()
    {
        LibraryItem.OnLibraryItemCreated -= OnLayoutChanged;
        LibraryItem.OnLibraryItemDestroyed -= OnLayoutChanged;
        PortConnectionController.OnConnectionCreated -= OnLayoutChanged;
        PortConnectionController.OnConnectionDestroyed -= OnLayoutChanged;
    }

    private void OnLayoutChanged()
    {
        StartCoroutine(ValidationCoroutine());
    }

    private IEnumerator ValidationCoroutine()
    {
        yield return null;
        processController.ValidateLayout();
    }
}
