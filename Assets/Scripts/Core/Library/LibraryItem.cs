using System;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.Library
{
    public class LibraryItem : MonoBehaviour, ILibraryItem
    {
        public static event Action OnLibraryItemCreated;
        public static event Action<LibraryItem> OnLibraryItemDestroyed;

        [field: SerializeField]
        public LibraryItemType ItemType { get; private set; }
        [field: SerializeField]
        public string ItemName { get; private set; }
        [field: SerializeField]
        public LibraryCategory Category { get; private set; }
        [field: SerializeField]
        public float Size { get; private set; }
        [field: SerializeField]
        public Sprite Visualization { get; private set; }
        [field: SerializeField]
        public Port[] Ports { get; private set; }
        [field: SerializeField]
        public Renderer Renderer { get; private set; }

        public GameObject GameObject => gameObject;

        private void Awake()
        {
            OnLibraryItemCreated?.Invoke();
        }

        private void OnDestroy()
        {
            OnLibraryItemDestroyed?.Invoke(this);
        }
    }
}