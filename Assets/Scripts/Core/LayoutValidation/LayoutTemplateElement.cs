using System;
using UnityEngine;

[Serializable]
public class LayoutTemplateElement
{
    [field: SerializeField]
    public LibraryItemType LibraryItemType { get; private set; }
    [field: SerializeField]
    public LibraryItemType[] OutputPortsConnectedItems { get; private set; }
}
