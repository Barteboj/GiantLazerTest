using System;
using GiantLaserTest.Core.Ports;
using UnityEngine;

[Serializable]
public struct PortVisualData
{
    [field: SerializeField]
    public PortType PortType { get; private set; }
    [field: SerializeField]
    public Material Material { get; private set; }
}
