using System;
using UnityEngine;
using UnityEngine.UI;

public class TestEvaluator : MonoBehaviour, IEvaluator
{
    public EvaluationMode Mode => EvaluationMode.Test;

    [SerializeField]
    private Button evaluateButton;
    
    private ILayoutValidationProcessController processController;

    public void Activate(ILayoutValidationProcessController processController)
    {
        this.processController = processController;
        evaluateButton.onClick.AddListener(OnEvaluateButtonClicked);
    }

    public void Deactivate()
    {
        evaluateButton.onClick.RemoveListener(OnEvaluateButtonClicked);
    }

    private void OnEvaluateButtonClicked()
    {
        processController.ValidateLayout();
    }
}
