using System.Collections.Generic;
using GiantLaserTest.Core.Library;

namespace GiantLaserTest.Core.Save
{
    public struct LibraryItemDTO
    {
        public SerializableVector3 Position { get; private set; }
        public LibraryItemType ItemType { get; private set; }
        public List<PortDTO> OutputPortsConnections { get; private set; }

        public LibraryItemDTO(SerializableVector3 position, LibraryItemType itemType, List<PortDTO> outputPortsConnections)
        {
            Position = position;
            ItemType = itemType;
            OutputPortsConnections = outputPortsConnections;
        }
    }
}