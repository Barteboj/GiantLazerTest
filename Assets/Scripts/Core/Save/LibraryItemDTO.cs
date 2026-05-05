using UnityEngine;

public class LibraryItemDTO
{
    public SerializableVector3 Position { get; set; }
    public LibraryItemType ItemType { get; set; }
    public PortDTO[] OutputPortsConnections { get; set; }
}
