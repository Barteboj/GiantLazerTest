using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LibraryItemButtonController : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI DescriptionText { get; private set; }

    [SerializeField]
    private Button button;

    private LibraryItem libraryItem;

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
        Instantiate(libraryItem.Prefab);
    }

    public void Initialize(LibraryItem item)
    {
        libraryItem = item;
        DescriptionText.text = $"{libraryItem.ItemName} {libraryItem.Category}";
    }
}
