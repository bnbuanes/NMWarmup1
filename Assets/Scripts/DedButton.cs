using UnityEngine;
using UnityEngine.SceneManagement;

public class DedButton : MonoBehaviour {
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}