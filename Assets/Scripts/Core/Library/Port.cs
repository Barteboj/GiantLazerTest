using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    [field: SerializeField]
    public PortType Type { get; private set; }
    [field: SerializeField]
    public string PortName {get; private set; }
    [field: SerializeField]
    public List<LibraryCategory> CompatibleCategories { get; private set; }
    [field: SerializeField]
    public List<LibraryItemType> CompatibleItems { get; private set; }
    [field: SerializeField]
    public Port connectedPort { get; private set; }
}