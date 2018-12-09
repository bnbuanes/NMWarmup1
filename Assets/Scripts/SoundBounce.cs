using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SoundBounce : MonoBehaviour {
    
    public StudioEventEmitter emitter;
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            BounceSound();
        }
    }

    private void BounceSound() {
        emitter.Play();
    }
}