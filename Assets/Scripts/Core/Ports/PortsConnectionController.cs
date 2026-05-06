using System;
using UnityEngine;

namespace GiantLaserTest.Core.Ports
{
    public class PortsConnectionController : MonoBehaviour
    {
        public static event Action ConnectionCreated;
        public static event Action ConnectionDestroyed;

        [Header("References")]
        [SerializeField]
        private LineRenderer lineRenderer;

        private void OnDisable()
        {
            ConnectionDestroyed?.Invoke();
        }

        public void Initialize(Port portA, Port portB)
        {
            lineRenderer.SetPosition(0, portA.transform.position);
            lineRenderer.SetPosition(1, portB.transform.position);
            portA.SetupConnection(this, portB);
            portB.SetupConnection(this, portA);
            ConnectionCreated?.Invoke();
        }
    }
}