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

    private LibraryItem itemPrefab;

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
        Instantiate(itemPrefab);
    }

    public void Initialize(LibraryItem itemPrefab)
    {
        this.itemPrefab = itemPrefab;
        DescriptionText.text = $"{this.itemPrefab.ItemName} {this.itemPrefab.Category}";
    }
}
