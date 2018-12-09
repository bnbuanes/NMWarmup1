using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundBounce : MonoBehaviour {

    public float distanceToDelay = 1.5f;
    public float minDelay = .5f;
    public float maxDelay = 5f;
    
    public StudioEventEmitter emitter;
    public List<SurfaceTypeToEmitter> emittersForSurfaces;

    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            BounceSound();
        }
    }

    private void BounceSound() {
        emitter.Play();

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(.5f, .5f)), out var hit)) {
            var surface = hit.collider.GetComponent<Surface>();
            if (surface != null) {
                switch (surface) {
                    case Surface_Emitter surfaceEmitter:
                        HandleHitSurfaceEmitter(surfaceEmitter, hit);
                        break;
                    case Surface_Type surfaceType:
                        HandleHitSurfaceType(surfaceType, hit);
                        break;

                }
            }
        }
    }

    private void HandleHitSurfaceEmitter(Surface_Emitter surfaceEmitter, RaycastHit hit) {
        StartCoroutine(PlayEcho(surfaceEmitter.emitter, hit));
    }

    private void HandleHitSurfaceType(Surface_Type surface, RaycastHit hit) {
        foreach (var emForSur in emittersForSurfaces) {
            if (emForSur.type == surface.surfaceType) {

                StartCoroutine(PlayEcho(emForSur.emitter, hit));
            }
        }
    }

    private IEnumerator PlayEcho(StudioEventEmitter emitter, RaycastHit hit) {
        var delay = CalculateDelay(hit);
        if (delay > maxDelay)
            yield break;

        StartCoroutine(SimulateEcho(transform.position, hit.point, delay, hit.collider));
        
        yield return new WaitForSeconds(delay);
        emitter.Play();
    }

    private IEnumerator SimulateEcho(Vector3 @from, Vector3 to, float duration, Collider hitThing) {
        var simulation = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        simulation.position = from;

        simulation.transform.localScale = Vector3.one * .5f;
        simulation.GetComponent<MeshRenderer>().material.color = Color.red;
        Destroy(simulation.GetComponent<Collider>());

        var durationOver = duration / 2f;
        var startTime = Time.time;
        var endTime = Time.time + durationOver;

        var lerpVal = 0f;

        do {
            yield return null;

            lerpVal = (Time.time - startTime) / (endTime - startTime);
            
            simulation.position = Vector3.Lerp(from, to, lerpVal);
            //steffen
            emitter.transform.position = simulation.position;
            //steffen
            
        } while (lerpVal < 1f);

        startTime = endTime;
        endTime += durationOver;

        var rend = hitThing.GetComponent<MeshRenderer>();
        if (rend)
            simulation.GetComponent<MeshRenderer>().sharedMaterial = rend.sharedMaterial;

        do {
            yield return null;

            lerpVal = (Time.time - startTime) / (endTime - startTime);
            
            simulation.position = Vector3.Lerp(to, transform.position, lerpVal);
            //steffen
            emitter.transform.position = simulation.position;
            //steffen

        } while (lerpVal < 1f);

        //steffen
        emitter.Stop();
        //steffen
        Destroy(simulation.gameObject);
    }

    private float CalculateDelay(RaycastHit hit) {
        var distance = hit.distance;

        var value = distance * distanceToDelay;

        var delay = value;
        if (delay < minDelay)
            delay = minDelay;
        
        return delay;
    }
}


[Serializable]
public class SurfaceTypeToEmitter {
    public SurfaceType type;
    public StudioEventEmitter emitter;
}