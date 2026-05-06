using System;
using GiantLaserTest.Core.Library;
using TMPro;
using UnityEngine;

namespace GiantLaserTest.UI
{
    public class LibraryPanelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Dropdown categoryDropdown;
        [SerializeField]
        private Transform itemListContainer;
        [SerializeField]
        private GameObject[] libraryItemPrefabs;
        [SerializeField]
        private GameObject itemButtonPrefab;
        [SerializeField]
        private Transform spawnPoint;

        private LibraryCategory[] categories;

        private void Start()
        {
            categories = Enum.GetValues(typeof(LibraryCategory)) as LibraryCategory[];

            foreach (LibraryCategory category in categories)
            {
                categoryDropdown.options.Add(new TMP_Dropdown.OptionData(category.ToString()));
            }

            categoryDropdown.onValueChanged.AddListener(OnCategoryChanged);
            LoadItemsForCategory(categoryDropdown.value);
        }

        private void OnDestroy()
        {
            categoryDropdown.onValueChanged.RemoveListener(OnCategoryChanged);
        }

        private void OnCategoryChanged(int categoryIndex)
        {
            LoadItemsForCategory(categoryIndex);
        }

        private void LoadItemsForCategory(int categoryIndex)
        {
            var chosenCategory = categories[categoryIndex];

            foreach (Transform child in itemListContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var itemPrefab in libraryItemPrefabs)
            {
                var libraryItem = itemPrefab.GetComponent<ILibraryItem>();
                if (libraryItem.Category == chosenCategory)
                {
                    var instantiated = Instantiate(itemButtonPrefab, itemListContainer).GetComponent<LibraryItemButtonController>();
                    instantiated.Initialize(libraryItem, spawnPoint.position);
                }
            }
        }
    }
}