using UnityEngine;

public class DangerZone : MonoBehaviour {

    private void Awake() {
        var col = GetComponent<Collider>();
        if(col == null)
            col = gameObject.AddComponent<BoxCollider>();

        if (!col.isTrigger)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            player.Kill();
        }
    }

    public void SetClosest() { }
    public void SetNotClosest() { }
}