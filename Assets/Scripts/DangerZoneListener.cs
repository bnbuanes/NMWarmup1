using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DangerZoneListener : MonoBehaviour {
    public StudioEventEmitter dangerEmitter;
    public LayerMask layermask;
    public float lerpSpeed = 5f;

    private readonly Collider[] cols = new Collider[10];
    private DangerZone dangerZone;

    private void Update() {
        var numHits = Physics.OverlapSphereNonAlloc(transform.position, 10f, cols, layermask);
        Debug.Log(numHits);
        if (numHits > 0) {
            Array.Sort(cols, 0, numHits, new CompareByDistance { baseTransform = transform });

            var old = dangerZone;
            dangerZone = cols[0].transform.GetComponent<DangerZone>();

            if (old != dangerZone) {
                if(old != null)
                    old.SetNotClosest();
                dangerZone.SetClosest();
            }

        }
        else {
            return;
        }

        var target = dangerZone.transform.position;

        dangerEmitter.transform.position = Vector3.Lerp(dangerEmitter.transform.position, target, Time.deltaTime * lerpSpeed);
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