using System.Collections.Generic;
using UnityEngine;

public interface IDeskController
{
    List<ILibraryItem> LibraryItems { get; }
}
