using System.Collections;
using FMODUnity;
using UnityEngine;

public class TestSoundDirection : MonoBehaviour {
    public float delayBeforeStart;
    public StudioEventEmitter emitter;

    IEnumerator Start() {
        yield return new WaitForSeconds(delayBeforeStart);
        StartCoroutine(PlaySound());

    }

    private IEnumerator PlaySound() {
        while (true) {
            emitter.Play();
            yield return new WaitForSeconds(1f);
        }
    }
}