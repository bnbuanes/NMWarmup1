using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    Vector2 input { get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); } }
    Vector2 mouseInput { get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); } }

    Rigidbody rb { get { return GetComponent<Rigidbody>(); } }
    float walkSpeed = 3;

    public Transform horBase, verBase;

    private void FixedUpdate() {
        rb.velocity = transform.right * input.x * walkSpeed + transform.forward * walkSpeed * input.y + transform.up * rb.velocity.y;

        horBase.Rotate(Vector3.up, mouseInput.x);
        verBase.Rotate(Vector3.right, -mouseInput.y);
    }
}
