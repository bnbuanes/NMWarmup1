using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerController : MonoBehaviour {
    Vector2 input { get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); } }
    Vector2 mouseInput { get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); } }

    Rigidbody rb { get { return GetComponent<Rigidbody>(); } }
    float walkSpeed = 3;

    public Transform horBase, verBase;

    float verX = 0;
    private bool walking;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() {
        rb.velocity = horBase.right * input.x * walkSpeed + horBase.forward * walkSpeed * input.y + transform.up * rb.velocity.y;

        if (rb.velocity.magnitude > 0.1f && !walking)
            StartCoroutine(PlayWalkSound());
        else if (rb.velocity.magnitude < 0.1f) {
            walking = false;
            StopAllCoroutines();
        }
    }

    private void Update() {

        horBase.Rotate(Vector3.up, mouseInput.x);
        //verBase.Rotate(Vector3.right, -mouseInput.y);

        verX -= mouseInput.y;
        verX = Mathf.Clamp(verX, -90, 90);

        verBase.localRotation = Quaternion.Euler(verX, 0, 0);

        if(Input.GetKeyDown(KeyCode.P))
            GetComponent<StudioEventEmitter>().Play();

        /*
        float x = verBase.localRotation.eulerAngles.x;

        if (x > 0 && x < 90 && mouseInput.y < 0)
            verBase.Rotate(Vector3.right, -mouseInput.y);

        if (x < 360 && x > 270 && mouseInput.y > 0)
            verBase.Rotate(Vector3.right, -mouseInput.y);
            */

        //verBase.rotation.eulerAngles.x = Mathf.Clamp(verBase.rotation.eulerAngles.x, -90, 90);



    }

    IEnumerator PlayWalkSound() {
        walking = true;
        while (true) {

            GetComponent<StudioEventEmitter>().Play();
            yield return new WaitForSeconds(1);
        }
    }
}
