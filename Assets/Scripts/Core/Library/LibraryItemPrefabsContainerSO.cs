using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LibraryItemPrefabsContainerSO", menuName = "ScriptableObjects/LibraryItemPrefabsContainerSO")]
public class LibraryItemPrefabsContainerSO : ScriptableObject, ILibraryItemPrefabsContainer
{
    [field: SerializeField]
    public List<GameObject> LibraryItemPrefabs { get; private set; }
}
