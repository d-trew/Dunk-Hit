using UnityEngine;
using UnityEngine.UI; // Required for UI components
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHoop; // Reference to the right hoop GameObject
    [SerializeField] private GameObject leftHoop;  // Reference to the left hoop GameObject
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool gameEnded = false; // Track if the game has ended

    // Define the range for random hoop heights
    [SerializeField] private float minHoopHeight = -4f; // Minimum Y position
    [SerializeField] private float maxHoopHeight = 2f;  // Maximum Y position

    // Timer variables
    [SerializeField] private float initialTimeLimit = 10f; // Initial time limit for scoring
    [SerializeField] private float minimumTimeLimit = 3f;  // Minimum time limit after decreasing
    [SerializeField] private float timeDecreaseAmount = 1f; // Amount to decrease the time limit after each score
    private float currentTimer; // Current countdown timer
    private bool timerActive = false; // To track if the timer is currently running

    // UI components for visualizing the timer
    [SerializeField] private Slider timeBar; // Reference to the UI Slider representing the time
    [SerializeField] private Image fillImage; // Reference to the Fill Image component of the slider

    [SerializeField] private Color fullTimeColor = Color.green; // Color when time is full
    [SerializeField] private Color lowTimeColor = Color.red;    // Color when time is running out

    // Start is called before the first frame update
    void Start()
    {
        // Ensure only the right hoop is active at the start
        ActivateHoop("Right");

        // Initialize the time bar slider
        if (timeBar != null)
        {
            timeBar.maxValue = initialTimeLimit;
            timeBar.value = initialTimeLimit;

            // Initialize fill color
            if (fillImage != null)
            {
                fillImage.color = fullTimeColor;
            }
        }
    }

    // Method to activate the specified hoop
    public void ActivateHoop(string hoopSide)
    {
        float randomHeight = Random.Range(minHoopHeight, maxHoopHeight); // Generate a random height within the range

        if (hoopSide == "Right")
        {
            rightHoop.SetActive(true);
            leftHoop.SetActive(false);
            SetHoopPosition(rightHoop, randomHeight);
            Debug.Log("Right hoop activated at height: " + rightHoop.transform.localPosition.y);
        }
        else if (hoopSide == "Left")
        {
            randomHeight = Random.Range(minHoopHeight, maxHoopHeight); // Generate a new random height for the left hoop
            rightHoop.SetActive(false);
            leftHoop.SetActive(true);
            SetHoopPosition(leftHoop, randomHeight);
            Debug.Log("Left hoop activated at height: " + leftHoop.transform.localPosition.y);
        }

        // Start or reset the timer after a hoop is activated
        StartTimer();
    }

    private void SetHoopPosition(GameObject hoop, float height)
    {
        // Set the hoop to the exact position
        hoop.transform.localPosition = new Vector2(hoop.transform.localPosition.x, height);
    }

    // Method to handle scoring
    public void OnHoopScored(GameObject scoredHoop)
    {
        if (scoredHoop == rightHoop)
        {
            Debug.Log("Scored in the right hoop.");
            ActivateHoop("Left");
        }
        else if (scoredHoop == leftHoop)
        {
            Debug.Log("Scored in the left hoop.");
            ActivateHoop("Right");
        }

        // Decrease the time limit after each score, but not below the minimum time limit
        initialTimeLimit = Mathf.Max(initialTimeLimit - timeDecreaseAmount, minimumTimeLimit);
        StartTimer(); // Restart the timer after scoring
    }

    // Method to start the timer
    private void StartTimer()
    {
        currentTimer = initialTimeLimit; // Reset the timer to the current time limit
        timerActive = true; // Activate the timer

        // Update the time bar to reflect the new time limit
        if (timeBar != null)
        {
            timeBar.maxValue = initialTimeLimit;
            timeBar.value = initialTimeLimit;
        }
    }

    // Method to end the game
    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true; // Set the game ended flag
            Debug.Log("Game Over! Time ran out.");

            // Optionally, add logic to stop the game or reset the scene
            Time.timeScale = 0; // Freeze the game

            // Show the game over screen
            if (gameOverScreen != null)
            {
                gameOverScreen.ShowGameOver(int.Parse(scoreText.text)); // Pass final score if you have one
            }
        }
    }

    void Update()
    {
        if (timerActive && !gameEnded)
        {
            currentTimer -= Time.deltaTime;

            if (timeBar != null)
            {
                timeBar.value = currentTimer;

                // Update the color based on the time remaining
                if (fillImage != null)
                {
                    fillImage.color = Color.Lerp(lowTimeColor, fullTimeColor, currentTimer / initialTimeLimit);
                }
            }

            if (currentTimer <= 0)
            {
                EndGame();
            }
        }

        Debug.Log($"Right hoop current position: {rightHoop.transform.localPosition.y}");
        Debug.Log($"Left hoop current position: {leftHoop.transform.localPosition.y}");
        Debug.Log($"Current timer: {currentTimer}");
    }
}
