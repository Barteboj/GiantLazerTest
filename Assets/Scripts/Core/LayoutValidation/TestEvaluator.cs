using UnityEngine;
using UnityEngine.UI;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class TestEvaluator : MonoBehaviour, IEvaluator
    {
        public EvaluationMode Mode => EvaluationMode.Test;

        [Header("References")]
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
}