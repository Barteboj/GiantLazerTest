using GiantLaserTest.Core.Library;

namespace GiantLaserTest.Core.Save
{
    public struct PortDTO
    {
        public LibraryItemType ConnectedItemType { get; private set; }
        public int ConnectedPortIndex { get; private set; }

        public PortDTO(LibraryItemType connectedItemType, int connectedPortIndex)
        {
            ConnectedItemType = connectedItemType;
            ConnectedPortIndex = connectedPortIndex;
        }
    }
}