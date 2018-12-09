using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    Vector2 input { get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); } }
    Vector2 mouseInput { get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); } }

    Rigidbody rb { get { return GetComponent<Rigidbody>(); } }
    
    public float walkSpeed = 3;
    public float stepDistance = 1f;

    public Transform horBase, verBase;

    public StudioEventEmitter stepEmitter;
    public StudioEventEmitter dedEmitter;

    float verX = 0;
    private bool walking;

    private Vector3 lastStepPosition;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        lastStepPosition = transform.position;
    }

    private void FixedUpdate() {
        rb.velocity = horBase.right * input.x * walkSpeed + horBase.forward * walkSpeed * input.y + transform.up * rb.velocity.y;
    }

    private void Update() {
        horBase.Rotate(Vector3.up, mouseInput.x);

        verX -= mouseInput.y;
        verX = Mathf.Clamp(verX, -90, 90);

        verBase.localRotation = Quaternion.Euler(verX, 0, 0);

        Footsteps();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Footsteps() {
        var grounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        if (!grounded)
            return;
        
        var position = transform.position;

        var distance = Vector3.Distance(position, lastStepPosition);
        
        if (distance > stepDistance) {
            stepEmitter.Play();

            lastStepPosition = lastStepPosition + ((position - lastStepPosition).normalized * stepDistance);
        }
    }

    public void Kill() {
        Destroy(this);
        dedEmitter.Play();
        FindObjectOfType<DedCanvas>().Enable();
    }
}
