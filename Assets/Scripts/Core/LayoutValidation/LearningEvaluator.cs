using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class LearningEvaluator : MonoBehaviour, IEvaluator
    {
        public EvaluationMode Mode => EvaluationMode.Learning;

        [Header("References")]
        [SerializeField]
        private DeskController deskController;

        private ILayoutValidationProcessController processController;

        public void Activate(ILayoutValidationProcessController processController)
        {
            this.processController = processController;
            deskController.OnLibraryItemAdded += OnLayoutChanged;
            deskController.OnLibraryItemRemoved += OnLayoutChanged;
            PortsConnectionController.ConnectionCreated += OnLayoutChanged;
            PortsConnectionController.ConnectionDestroyed += OnLayoutChanged;
            processController.ValidateLayout();
        }

        public void Deactivate()
        {
            deskController.OnLibraryItemAdded -= OnLayoutChanged;
            deskController.OnLibraryItemRemoved -= OnLayoutChanged;
            PortsConnectionController.ConnectionCreated -= OnLayoutChanged;
            PortsConnectionController.ConnectionDestroyed -= OnLayoutChanged;
        }

        private void OnLayoutChanged()
        {
            processController.ValidateLayout();
        }
    }
}