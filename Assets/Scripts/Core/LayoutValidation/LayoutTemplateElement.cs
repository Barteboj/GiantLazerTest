using System;
using GiantLaserTest.Core.Library;
using UnityEngine;

namespace GiantLaserTest.Core.LayoutValidation
{
    [Serializable]
    public class LayoutTemplateElement
    {
        [field: SerializeField]
        public LibraryItemType LibraryItemType { get; private set; }
        [field: SerializeField]
        public LibraryItemType[] OutputPortsConnectedItems { get; private set; }
    }
}