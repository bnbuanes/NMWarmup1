using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundBounce : MonoBehaviour {

    public float distanceToDelay = 1.5f;
    public float minDelay = .5f;
    public float maxDelay = 1f;
    
    public StudioEventEmitter emitter;
    public List<SurfaceTypeToEmitter> emittersForSurfaces;

    private Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            BounceSound();
        }
    }

    private void BounceSound() {
        emitter.Play();

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit)) {
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

        StartCoroutine(SimulateEcho(transform.position, hit.point, delay));
        
        yield return new WaitForSeconds(delay);
        emitter.Play();
    }

    private IEnumerator SimulateEcho(Vector3 from, Vector3 to, float duration) {
        var simulation = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        simulation.position = from;

        simulation.transform.localScale = Vector3.one * .5f;
        simulation.GetComponent<MeshRenderer>().material.color = Color.red;

        var durationOver = duration / 2f;
        var startTime = Time.time;
        var endTime = Time.time + durationOver;

        var lerpVal = 0f;

        do {
            yield return null;

            lerpVal = (Time.time - startTime) / (endTime - startTime);
            
            simulation.position = Vector3.Lerp(from, to, lerpVal);
            
        } while (lerpVal < 1f);

        startTime = endTime;
        endTime += durationOver;

        do {
            yield return null;

            lerpVal = (Time.time - startTime) / (endTime - startTime);
            
            simulation.position = Vector3.Lerp(to, from, lerpVal);
            
        } while (lerpVal < 1f);
        
        Destroy(simulation.gameObject);
    }

    private float CalculateDelay(RaycastHit hit) {
        var distance = hit.distance;

        return Mathf.Clamp(distance * distanceToDelay, minDelay, maxDelay);
    }
}


[Serializable]
public class SurfaceTypeToEmitter {
    public SurfaceType type;
    public StudioEventEmitter emitter;
}