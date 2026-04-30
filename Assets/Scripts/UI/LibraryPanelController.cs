using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LibraryPanelController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown categoryDropdown;
    [SerializeField]
    private Transform itemListContainer;
    [SerializeField]
    private LibraryItem[] libraryItems;
    [SerializeField]
    private GameObject itemButtonPrefab;

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

    private void OnCategoryChanged(int value)
    {
        LoadItemsForCategory(value);
    }

    private void LoadItemsForCategory(int categoryIndex)
    {
        var chosenCategory = categories[categoryIndex];

        foreach (Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in libraryItems)
        {
            if (item.Category == chosenCategory)
            {
                var instantiated = Instantiate(itemButtonPrefab, itemListContainer).GetComponent<LibraryItemButtonController>();
                instantiated.Initialize(item);
            }
        }
    }
}
