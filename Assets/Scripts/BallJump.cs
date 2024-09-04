using UnityEngine;
using TMPro;

public class PlayerJump : MonoBehaviour
{
    // Variables for jump height and movement
    public float jumpForce = 10f; // How high the ball should jump
    public float directionalVelocity = 2f; // Velocity in the X direction for each jump

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private GameManager gameManager; // Reference to the GameManager script
    private bool movingRight = true; // Flag to determine if the ball should move right or left
    private bool gameStarted = false; // To check if the game has started

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    // Perfect shot settings
    [SerializeField] private float perfectShotSpeed = 7f; // Threshold speed for a perfect shot
    private int perfectShotStreak = 0; // Tracks the number of consecutive perfect shots

    // Particle and visual effects
    [SerializeField] private TrailRenderer streakTrailEffect; // Reference to the particle trail effect
    [SerializeField] private GameObject screenFlashEffect; // Reference to a screen flash effect (like an overlay)

    // Positions for teleporting when hitting walls
    [SerializeField] private Transform leftWall; // Reference to the position of the left wall
    [SerializeField] private Transform rightWall; // Reference to the position of the right wall

    // Cooldown flag for teleportation
    private bool teleportCooldown = false; // Flag to manage teleport cooldown
    [SerializeField] private float teleportCooldownDuration = 0.1f; // Duration of cooldown to prevent re-triggering

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        rb.transform.position = new Vector2(0, 0);
        // Find and get a reference to the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();

        // Activate the right hoop at the start
        gameManager.ActivateHoop("Right");

        // Set the initial direction to right
        movingRight = true;

        // Set gravity to 0 at the start
        rb.gravityScale = 0;

        scoreText.text = "0";
        Debug.Log("Game started. Ball will move to the right.");

        // Ensure effects are disabled at the start
        if (streakTrailEffect != null) streakTrailEffect.emitting = false;
        if (screenFlashEffect != null) screenFlashEffect.SetActive(false);
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
        if (teleportCooldown) return; // Skip trigger handling if cooldown is active

        if (collision.CompareTag("Hoop"))
        {
            Debug.Log("Ball entered a hoop: " + collision.gameObject.name);
            rb.velocity = new Vector2(0, rb.velocity.y);

            // Check if the current shot is a "perfect" shot
            if (rb.velocity.magnitude >= perfectShotSpeed)
            {
                Debug.Log("Perfect shot!");

                // Flash the screen
                FlashScreen();

                // Award bonus points for a perfect shot
                IncreaseScore(3); // Adding bonus points
                perfectShotStreak++;

                // Activate streak effect if 3 or more perfect shots in a row
                if (perfectShotStreak >= 3 && streakTrailEffect != null)
                {
                    streakTrailEffect.emitting = true;
                }
            }
            else
            {
                // Reset streak if it's not a perfect shot
                perfectShotStreak = 0;
                if (streakTrailEffect != null) streakTrailEffect.emitting = false;
                IncreaseScore(1); // Regular points for normal shots
            }

            // Reverse the direction of the ball
            ReverseDirection();

            // Notify the HoopManager that a score has occurred
            gameManager.OnHoopScored(collision.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Ball hit a wall: " + collision.gameObject.name);

            // Teleport the ball to the opposite wall
            if (collision.gameObject.name == "leftWall")
            {
                rb.position = new Vector2(rightWall.position.x, rb.position.y); // Teleport to the right wall
            }
            else if (collision.gameObject.name == "rightWall")
            {
                rb.position = new Vector2(leftWall.position.x, rb.position.y); // Teleport to the left wall
            }

            // Activate cooldown to prevent immediate re-trigger
            teleportCooldown = true;
            Invoke("ResetTeleportCooldown", teleportCooldownDuration); // Set the cooldown period
        }
    }

    // Method to reset the teleport cooldown
    private void ResetTeleportCooldown()
    {
        teleportCooldown = false; // Allow collisions again
    }

    // Method to increase the score
    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = $"{score}"; // Update the score text
        Debug.Log("Score increased: " + score); // Debug log for checking score updates
    }

    // Method to flash the screen for visual feedback on perfect shots
    private void FlashScreen()
    {
        if (screenFlashEffect != null)
        {
            screenFlashEffect.SetActive(true);
            Invoke("DisableFlash", 0.1f); // Flash duration
        }
    }

    // Method to disable the screen flash effect
    private void DisableFlash()
    {
        if (screenFlashEffect != null)
        {
            screenFlashEffect.SetActive(false);
        }
    }
}
