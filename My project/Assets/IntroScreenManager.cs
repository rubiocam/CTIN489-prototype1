using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenManager : MonoBehaviour
{
    private string mainGameSceneName = "GAMEPLAY"; // Name of your main game scene

    void Update()
    {
        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Load the main game scene
            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}
