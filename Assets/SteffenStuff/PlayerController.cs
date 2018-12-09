using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
    private Vector2 input      => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    private Vector2 mouseInput => new Vector2(Input.GetAxis("Mouse X"),    Input.GetAxis("Mouse Y"));

    private Rigidbody rb;

    [FormerlySerializedAs("walkSpeed")]
    public float runSpeed = 7f;
    public float strafeSpeed = 3f;
    public float stepDistance = 1.5f;
    public float jumpForce = 5f;
    public float accelSpeed = 30f;

    public Transform horBase, verBase;

    public StudioEventEmitter stepEmitter;
    public StudioEventEmitter dedEmitter;

    private float verX = 0;
    private bool jumping;

    private Vector3 lastStepPosition;
    private bool checkIfLanded;
    private bool justLanded;
    private Vector3 landDirection;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        lastStepPosition = transform.position;
    }

    private void FixedUpdate() {
        if (jumping) {
            return;
        }

        if (justLanded) {
            rb.velocity = landDirection * runSpeed;
            justLanded = false;
            return;
        }

        var currentVelocity = rb.velocity;
        var movement = input.normalized;

        var wantedXZ = horBase.TransformDirection(new Vector3(movement.x * strafeSpeed, 0f, movement.y * runSpeed));
        
        var actualXZ = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        var adjustedXZ = Vector3.MoveTowards(actualXZ, wantedXZ, Time.deltaTime * accelSpeed);

        var direction = new Vector3(adjustedXZ.x, currentVelocity.y, adjustedXZ.z);

        rb.velocity = direction;

        
    }

    private void Update() {
        horBase.Rotate(Vector3.up, mouseInput.x);

        verX -= mouseInput.y;
        verX = Mathf.Clamp(verX, -90, 90);

        verBase.localRotation = Quaternion.Euler(verX, 0, 0);

        if (jumping && checkIfLanded) {
            jumping = !Physics.Raycast(transform.position, Vector3.down, 1.2f);
            if (!jumping) {
                PlayLandSound();
                lastStepPosition = transform.position;
                justLanded = true;
                landDirection = rb.velocity;
                landDirection.y = 0;
                landDirection = landDirection.normalized;
            }
        }
        else if(!jumping) {
            Footsteps();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayLandSound() {
        stepEmitter.Play();
    }

    private void Jump() {
        if (jumping)
            return;
        
        rb.velocity += new Vector3(0f, jumpForce, 0f);
        jumping = true;
        checkIfLanded = false;
    }

    private void OnCollisionEnter(Collision _) {
        checkIfLanded = true;
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
