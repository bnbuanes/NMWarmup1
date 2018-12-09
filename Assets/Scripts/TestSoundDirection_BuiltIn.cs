using System.Collections;
using FMODUnity;
using UnityEngine;

public class TestSoundDirection_BuiltIn : MonoBehaviour {
    public float delayBeforeStart;
    public AudioSource source;
    public AudioClip clip;

    IEnumerator Start() {
        yield return new WaitForSeconds(delayBeforeStart);
        StartCoroutine(PlaySound());

    }

    private IEnumerator PlaySound() {
        while (true) {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(1f);
        }
    }
}