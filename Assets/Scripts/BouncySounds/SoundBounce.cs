using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundBounce : MonoBehaviour {
    
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
            var surface = hit.collider.GetComponent<Surface_Type>();
            if (surface != null) {
                foreach (var emForSur in emittersForSurfaces) {
                    if (emForSur.type == surface.surfaceType) {
                        
                        StartCoroutine(PlayEcho(emForSur.emitter, hit));
                    }
                }
            }
        }
    }

    private IEnumerator PlayEcho(StudioEventEmitter emitter, RaycastHit hit) {
        yield return new WaitForSeconds(1f);
        Debug.Log("playing " + emitter.name);
        emitter.Play();
    }
}


[Serializable]
public class SurfaceTypeToEmitter {
    public SurfaceType type;
    public StudioEventEmitter emitter;
}