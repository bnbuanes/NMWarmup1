using UnityEngine;

public class DangerZone : MonoBehaviour {
    private MeshRenderer mr;

    private static Material defaultMat;
    private static Material closestMat;

    private void Awake() {
        mr = GetComponent<MeshRenderer>();
        if (defaultMat == null) {
            defaultMat = mr.sharedMaterial;
            closestMat = new Material(defaultMat);
            closestMat.color = Color.red;
        }
    }

    public void SetClosest() => mr.sharedMaterial = closestMat;
    public void SetNotClosest() => mr.sharedMaterial = defaultMat;
}