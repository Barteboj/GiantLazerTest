using UnityEngine;
using System;

namespace GiantLaserTest.Attributes
{
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public Type InterfaceType { get; private set; }

        public RequireInterfaceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}