using System;
using System.Collections.Generic;
using GiantLaserTest.Core.Ports;
using TMPro;
using UnityEngine;

namespace GiantLaserTest.Core.Library
{
    public class LibraryItem : MonoBehaviour, ILibraryItem
    {
        public static event Action<LibraryItem> ItemDestroyed;

        [field: Header("Parameters")]
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

        [field: Header("References")]
        [field: SerializeField]
        public List<Port> Ports { get; private set; }
        [field: SerializeField]
        public Renderer Renderer { get; private set; }
        [SerializeField]
        private TextMeshPro itemNameText;

        public GameObject GameObject => gameObject;
        public bool IsLockedInPlace { get; private set; }

        private void Awake()
        {
            itemNameText.SetText(ItemName);
        }

        private void OnDestroy()
        {
            ItemDestroyed?.Invoke(this);
        }

        public void LockInPlace()
        {
            IsLockedInPlace = true;
        }
    }
}