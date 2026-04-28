using UnityEngine;

[CreateAssetMenu(fileName = "LibraryItem", menuName = "Scriptable Objects/LibraryItem")]
public class LibraryItem : ScriptableObject
{
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