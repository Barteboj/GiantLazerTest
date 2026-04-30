using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Port
{
    [field: SerializeField]
    public PortType Type { get; private set; }
    [field: SerializeField]
    public string PortName {get; private set; }
    [field: SerializeField]
    public List<LibraryCategory> CompatibleCategories { get; private set; }
    [field: SerializeField]
    public List<LibraryItem> CompatibleItems { get; private set; }
}
