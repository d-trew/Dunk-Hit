using UnityEngine;

public class ShadowFollowX : MonoBehaviour
{
    [SerializeField] private Transform ball; // Reference to the ball's transform
    [SerializeField] private float fixedYPosition = -4.75f; // Set this to the desired y-coordinate for the shadow

    void Update()
    {
        if (ball != null)
        {
            // Update the shadow's position to match the ball's x coordinate, keeping the y position fixed
            transform.position = new Vector3(ball.position.x, fixedYPosition, transform.position.z);
        }
    }
}
