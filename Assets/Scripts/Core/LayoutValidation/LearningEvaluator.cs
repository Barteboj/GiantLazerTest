using System.Collections;
using UnityEngine;

public class LearningEvaluator : MonoBehaviour, IEvaluator
{
    public EvaluationMode Mode => EvaluationMode.Learning;

    [SerializeField]
    private DeskController deskController;

    private ILayoutValidationProcessController processController;

    public void Activate(ILayoutValidationProcessController processController)
    {
        this.processController = processController;

        deskController.OnLibraryItemAdded += OnLayoutChanged;
        deskController.OnLibraryItemRemoved += OnLayoutChanged;
        PortConnectionController.OnConnectionCreated += OnLayoutChanged;
        PortConnectionController.OnConnectionDestroyed += OnLayoutChanged;

        processController.ValidateLayout();
    }

    public void Deactivate()
    {
        deskController.OnLibraryItemAdded -= OnLayoutChanged;
        deskController.OnLibraryItemRemoved -= OnLayoutChanged;
        PortConnectionController.OnConnectionCreated -= OnLayoutChanged;
        PortConnectionController.OnConnectionDestroyed -= OnLayoutChanged;
    }

    private void OnLayoutChanged()
    {
        processController.ValidateLayout();
    }
}
