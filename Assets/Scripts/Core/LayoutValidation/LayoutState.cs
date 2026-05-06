using System.Collections.Generic;
using GiantLaserTest.Core.Library;

namespace GiantLaserTest.Core.LayoutValidation
{
    public class LayoutState
    {
        public List<ILibraryItem> LibraryItems { get; private set; }

        public LayoutState(List<ILibraryItem> libraryItems)
        {
            LibraryItems = libraryItems;
        }
    }
}