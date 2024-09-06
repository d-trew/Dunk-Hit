using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to start the game
    public void StartGame()
    {
        // Load the main game scene (adjust the scene name as needed)
        SceneManager.LoadScene("Game");
    }

    // Method to open options (optional)
    public void OpenOptions()
    {
        // Logic to open the options menu can be added here
        Debug.Log("Options Menu Opened");
    }

}
