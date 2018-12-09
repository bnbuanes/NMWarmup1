using FMODUnity;
using UnityEngine;

public class DangerZoneListener : MonoBehaviour {

    public Transform dangerZone;
    public StudioEventEmitter dangerEmitter;

    private Collider[] cols = new Collider[10];

    private void Update() {
        Physics.OverlapSphereNonAlloc(transform.position, 10f, cols);
    }
}

