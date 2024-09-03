using UnityEngine;
using TMPro;

public class PlayerJump : MonoBehaviour
{
    // Variables for jump height and movement
    public float jumpForce = 5f; // How high the ball should jump
    public float directionalVelocity = 2.5f; // Velocity in the X direction for each jump

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private GameManager gameManager; // Reference to the HoopManager script
    private bool movingRight = true; // Flag to determine if the ball should move right or left
    private bool gameStarted = false; // To check if the game has started

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();

        // Find and get a reference to the HoopManager in the scene
        gameManager = FindObjectOfType<GameManager>();

        // Activate the right hoop at the start
        gameManager.ActivateHoop("Right");

        // Set the initial direction to right
        movingRight = true;

        // Set gravity to 0 at the start
        rb.gravityScale = 0;

        scoreText.text = "0";
        Debug.Log("Game started. Ball will move to the right.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input (spacebar press) to make the ball jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                // If this is the first jump, enable gravity
                gameStarted = true;
                rb.gravityScale = 2; // Reset gravity to normal
                Debug.Log("Gravity enabled. Game started.");
            }

            Jump();
        }
    }

    // Method to make the ball jump
    void Jump()
    {
        // Determine the horizontal velocity based on the direction
        float horizontalVelocity = movingRight ? directionalVelocity : -directionalVelocity;

        // Apply an upward and directional force to the ball
        rb.velocity = new Vector2(horizontalVelocity, jumpForce);
        Debug.Log("Ball jumped with velocity: " + rb.velocity);
    }

    // Method to reverse ball direction
    public void ReverseDirection()
    {
        // Toggle the direction flag
        movingRight = !movingRight;
        Debug.Log("Direction reversed. Now moving " + (movingRight ? "right" : "left"));
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hoop"))
        {
            Debug.Log("Ball entered a hoop: " + collision.gameObject.name);
            rb.velocity = new Vector2(0, rb.velocity.y);
            IncreaseScore(1);
            // Reverse the direction of the ball
            ReverseDirection();

            // Notify the HoopManager that a score has occurred
            gameManager.OnHoopScored(collision.gameObject);
        } else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Ball hit a wall: " + collision.gameObject.name);

            // Call the GameManager to handle the end of the game
            gameManager.EndGame();
        }
    }

    // Method to increase the score
    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = $"{score}"; // Update the score text
        Debug.Log("Score increased: " + score); // Debug log for checking score updates
    }
}
