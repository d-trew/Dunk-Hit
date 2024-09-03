using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHoop; // Reference to the right hoop GameObject
    [SerializeField] private GameObject leftHoop;  // Reference to the left hoop GameObject
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    private bool gameEnded = false; // Track if the game has ended
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure only the right hoop is active at the start
        ActivateHoop("Right");
    }

    // Method to activate the specified hoop
    public void ActivateHoop(string hoopSide)
    {
        if (hoopSide == "Right")
        {
            rightHoop.SetActive(true);
            leftHoop.SetActive(false);
            Debug.Log("Right hoop activated.");
        }
        else if (hoopSide == "Left")
        {
            rightHoop.SetActive(false);
            leftHoop.SetActive(true);
            Debug.Log("Left hoop activated.");
        }
    }

    // Method to handle scoring
    public void OnHoopScored(GameObject scoredHoop)
    {
        if (scoredHoop == rightHoop)
        {
            // Disable the right hoop and activate the left hoop
            Debug.Log("Scored in the right hoop.");
            ActivateHoop("Left");
        }
        else if (scoredHoop == leftHoop)
        {
            // Disable the left hoop and activate the right hoop
            Debug.Log("Scored in the left hoop.");
            ActivateHoop("Right");
        }
    }
    // Method to end the game
    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true; // Set the game ended flag
            Debug.Log("Game Over! The ball hit a wall.");

            // Optionally, add logic to stop the game or reset the scene
            Time.timeScale = 0; // Freeze the game
        }
    }
}
