using System;
using UnityEngine;

public interface ILibraryItem
{
    GameObject GameObject { get; }
    string ItemName { get; }
    Port[] Ports { get; }
    LibraryItemType ItemType { get; }
    LibraryCategory Category { get; }
    Renderer Renderer { get; }
}
