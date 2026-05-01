using UnityEngine;

public class LibraryItem : MonoBehaviour
{
    [field: SerializeField]
    public LibraryItemType ItemType { get; private set; }
    [field: SerializeField]
    public string ItemName { get; private set; }
    [field: SerializeField]
    public LibraryCategory Category { get; private set; }
    [field: SerializeField]
    public float Size { get; private set; }
    [field: SerializeField]
    public Sprite Visualization { get; private set; }
    [field: SerializeField]
    public Port[] Ports { get; private set; }
}
