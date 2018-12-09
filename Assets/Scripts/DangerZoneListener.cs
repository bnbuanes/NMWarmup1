using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DangerZoneListener : MonoBehaviour {

    public StudioEventEmitter dangerEmitter;

    private Collider[] cols = new Collider[10];

    private void Update() {
        var numHits = Physics.OverlapSphereNonAlloc(transform.position, 10f, cols);

        Transform dangerZone;
        if (numHits > 0) {
            Array.Sort(cols, 0, numHits, new CompareByDistance { baseTransform = transform });
            dangerZone = cols[0].transform;
        }
        else {
            dangerZone = null;
        }


        var target = dangerZone == null ? transform.position : dangerZone.position;

        dangerEmitter.transform.position = Vector3.Lerp(dangerEmitter.transform.position, target, Time.deltaTime * 5f);
    }

    private class CompareByDistance : IComparer<Collider> {
        public Transform baseTransform;
        
        public int Compare(Collider x, Collider y) {
            var xPos = x.transform.position;
            var yPos = y.transform.position;

            return Vector3.Distance(xPos, baseTransform.position).CompareTo(Vector3.Distance(yPos, baseTransform.position));
        }
    }
}

