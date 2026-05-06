using GiantLaserTest.Core.Library;
using UnityEngine;

namespace GiantLaserTest.Core.Save
{
    public class LibraryItemDTO
    {
        public SerializableVector3 Position { get; set; }
        public LibraryItemType ItemType { get; set; }
        public PortDTO[] OutputPortsConnections { get; set; }
    }
}