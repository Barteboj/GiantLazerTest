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

    public void Initialize(ILibraryItem itemPrefab, Vector3 position)
    {
        this.itemPrefab = itemPrefab;
        DescriptionText.text = $"{this.itemPrefab.ItemName} {this.itemPrefab.Category}";
        spawnPosition = position;
    }
}
