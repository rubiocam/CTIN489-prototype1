using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }

    void RestartCurrentScene()
    {
        // Get the active scene and reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
