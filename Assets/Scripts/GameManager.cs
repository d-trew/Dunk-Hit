using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHoop; // Reference to the right hoop GameObject
    [SerializeField] private GameObject leftHoop;  // Reference to the left hoop GameObject
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    private bool gameEnded = false; // Track if the game has ended

    // Define the range for random hoop heights
    [SerializeField] private float minHoopHeight = -4f; // Minimum Y position
    [SerializeField] private float maxHoopHeight = 2f;  // Maximum Y position

    // Start is called before the first frame update
    void Start()
    {
        // Ensure only the right hoop is active at the start
        ActivateHoop("Right");
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

    void Update()
    {
        // Debugging positions continuously
        Debug.Log($"Right hoop current position: {rightHoop.transform.localPosition.y}");
        Debug.Log($"Left hoop current position: {leftHoop.transform.localPosition.y}");
    }
}
