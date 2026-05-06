using System;
using System.Collections.Generic;
using System.Linq;
using GiantLaserTest.Attributes;
using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.Library;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class LayoutValidationProcessController : MonoBehaviour, ILayoutValidationProcessController
    {
        public event Action<List<ValidationResult>> ValidationCompleted;

        [Header("References")]
        [SerializeField]
        private DeskController deskController;
        [SerializeField, RequireInterface(typeof(ILayoutValidator))]
        private UnityEngine.Object[] layoutValidatorsReference;

        private ILayoutValidator[] layoutValidators;

        private void Awake()
        {
            layoutValidators = layoutValidatorsReference.OfType<ILayoutValidator>().ToArray();
        }

        public void ValidateLayout()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            LayoutState layoutState = new LayoutState(new List<ILibraryItem>(deskController.LibraryItems));

            foreach (var validator in layoutValidators)
            {
                var result = validator.Validate(layoutState);
                validationResults.Add(result);
            }

            ValidationCompleted?.Invoke(validationResults);
        }
    }
}