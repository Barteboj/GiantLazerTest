using GiantLaserTest.Core.Library;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GiantLaserTest.UI
{
    public class LibraryItemButtonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Button button;
        [field: SerializeField]
        public TextMeshProUGUI DescriptionText { get; private set; }

        private ILibraryItem itemPrefab;
        private Vector3 spawnPosition;

        private void OnEnable()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Instantiate(itemPrefab.GameObject, spawnPosition, Quaternion.identity);
        }

        public void Initialize(ILibraryItem itemPrefab, Vector3 spawnPosition)
        {
            this.itemPrefab = itemPrefab;
            this.spawnPosition = spawnPosition;
            DescriptionText.text = $"{this.itemPrefab.ItemName} {this.itemPrefab.Category}";
        }
    }
}