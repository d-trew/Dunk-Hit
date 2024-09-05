using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;            // Reference to the score text UI element
    [SerializeField] private Button playAgainButton;      // Reference to the restart button
    [SerializeField] private Button mainMenuButton;     // Reference to the main menu button

    private void Start()
    {
        // Initially hide the Game Over screen
        gameObject.SetActive(false);

        // Add listeners to the buttons
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    // Method to show the Game Over screen
    public void ShowGameOver(int finalScore)
    {
        gameObject.SetActive(true);   // Activate the game over panel
        scoreText.text =  finalScore + " POINTS";  // Display the final score
        Time.timeScale = 0;  // Pause the game
    }

    // Method to restart the game
    public void PlayAgain()
    {
        Time.timeScale = 1;  // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the current scene
    }

    // Method to go back to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1;  // Resume the game
        SceneManager.LoadScene("MainMenu");  // Load the main menu scene (adjust the scene name as needed)
    }
}
