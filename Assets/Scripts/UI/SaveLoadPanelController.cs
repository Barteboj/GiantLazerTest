using UnityEngine;
using UnityEngine.UI;
using System.IO;
using GiantLaserTest.Attributes;
using GiantLaserTest.Core.Save;

namespace GiantLaserTest.UI
{
    public class SaveLoadPanelController : MonoBehaviour
    {
        private string saveFilePath;

        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button loadButton;
        [SerializeField, RequireInterface(typeof(ILayoutSaveController))]
        private Object layoutSaveControllerReference;

        private ILayoutSaveController layoutSaveController;

        private void Awake()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
            layoutSaveController = layoutSaveControllerReference as ILayoutSaveController;
        }

        private void OnEnable()
        {
            saveButton.onClick.AddListener(OnSaveClicked);
            loadButton.onClick.AddListener(OnLoadClicked);
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(OnSaveClicked);
            loadButton.onClick.RemoveListener(OnLoadClicked);
        }

        private void OnSaveClicked()
        {
            layoutSaveController.SaveLayout();
        }

        private void OnLoadClicked()
        {
            layoutSaveController.LoadLayout();
        }
    }
}