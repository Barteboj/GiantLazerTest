using System;
using System.Collections.Generic;
using GiantLaserTest.Core.Ports;
using UnityEngine;

namespace GiantLaserTest.Core.Library
{
    public class LibraryItem : MonoBehaviour, ILibraryItem
    {
        public static event Action<LibraryItem> ItemDestroyed;

        [field: SerializeField]
        public LibraryItemType ItemType { get; private set; }
        [field: SerializeField]
        public string ItemName { get; private set; }
        [field: SerializeField]
        public LibraryCategory Category { get; private set; }
        [field: SerializeField]
        public List<Port> Ports { get; private set; }
        public GameObject GameObject => gameObject;
        [field: SerializeField]
        public Renderer Renderer { get; private set; }
        [field: SerializeField]
        public float Size { get; private set; }
        [field: SerializeField]
        public Sprite Visualization { get; private set; }

        private void OnDestroy()
        {
            ItemDestroyed?.Invoke(this);
        }
    }
}