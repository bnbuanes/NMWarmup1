using UnityEngine;

public class DedCanvas : MonoBehaviour {
    public GameObject contentParent;
    public GameObject cursorCanvas;

    public void Enable() {
        contentParent.SetActive(true);
        cursorCanvas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Destroy(FindObjectOfType<SoundBounce>().gameObject);
    }
}