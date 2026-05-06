using System;
using UnityEngine;

public class PortConnectionController : MonoBehaviour
{
    public static event Action OnConnectionCreated;
    public static event Action OnConnectionDestroyed;

    [SerializeField]
    private LineRenderer lineRenderer;

    private Port portA;
    private Port portB;

    private void OnDisable()
    {
        OnConnectionDestroyed?.Invoke();
    }

    public void Initialize(Port portA, Port portB)
    {
        this.portA = portA;
        this.portB = portB;
        lineRenderer.SetPosition(0, portA.transform.position);
        lineRenderer.SetPosition(1, portB.transform.position);
        portA.SetupConnection(this, portB);
        portB.SetupConnection(this, portA);
        OnConnectionCreated?.Invoke();
    }

    private void Update()
    {
        if (portA != null && portB != null)
        {
            lineRenderer.SetPosition(0, portA.transform.position);
            lineRenderer.SetPosition(1, portB.transform.position);
        }
    }
}
