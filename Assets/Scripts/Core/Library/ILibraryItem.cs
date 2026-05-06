using System.Collections.Generic;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.Library
{
    public interface ILibraryItem
    {
        string ItemName { get; }
        LibraryItemType ItemType { get; }
        LibraryCategory Category { get; }
        List<Port> Ports { get; }
        GameObject GameObject { get; }
        Renderer Renderer { get; }
    }
}