using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Movement3D player = other.GetComponent<Movement3D>();
        if (player != null) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
