using UnityEngine;

public class RandomBoxMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f; // Speed of the sprite
    public float directionChangeInterval = 1f; // Time interval for changing direction

    private Vector3 startPosition; // The sprite's starting position
    private Vector2 moveDirection; // The current movement direction
    public float boundarySize = 50f; // Size of the box in pixels
    public float edgeBuffer = 0.5f; // Distance from the edge to adjust direction

    void Start()
    {
        // Store the sprite's initial position
        startPosition = transform.position;

        // Start the coroutine to change direction periodically
        StartCoroutine(ChangeDirection());
    }

    void Update()
    {
        // Move the sprite in the current direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check and adjust direction if near the edges
        AdjustDirectionAtEdges();

        // Clamp the sprite's position to stay within the 50x50 box
        ClampPosition();
    }

    private System.Collections.IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Randomize a new movement direction
            float angle = Random.Range(0f, 360f);
            moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            // Wait for the direction change interval
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void AdjustDirectionAtEdges()
    {
        // Convert the boundary size from pixels to world units
        float worldBoundarySize = boundarySize / 100f;

        // Calculate the min and max positions in the box
        float minX = startPosition.x - worldBoundarySize / 2;
        float maxX = startPosition.x + worldBoundarySize / 2;
        float minY = startPosition.y - worldBoundarySize / 2;
        float maxY = startPosition.y + worldBoundarySize / 2;

        // Adjust direction when close to the edges
        if (transform.position.x <= minX + edgeBuffer && moveDirection.x < 0)
            moveDirection.x = Mathf.Abs(moveDirection.x); // Bounce right
        if (transform.position.x >= maxX - edgeBuffer && moveDirection.x > 0)
            moveDirection.x = -Mathf.Abs(moveDirection.x); // Bounce left
        if (transform.position.y <= minY + edgeBuffer && moveDirection.y < 0)
            moveDirection.y = Mathf.Abs(moveDirection.y); // Bounce up
        if (transform.position.y >= maxY - edgeBuffer && moveDirection.y > 0)
            moveDirection.y = -Mathf.Abs(moveDirection.y); // Bounce down

        // Normalize direction to maintain consistent speed
        moveDirection = moveDirection.normalized;
    }

    private void ClampPosition()
    {
        // Convert the boundary size from pixels to world units
        float worldBoundarySize = boundarySize / 100f;

        // Calculate the min and max positions in the box
        float minX = startPosition.x - worldBoundarySize / 2;
        float maxX = startPosition.x + worldBoundarySize / 2;
        float minY = startPosition.y - worldBoundarySize / 2;
        float maxY = startPosition.y + worldBoundarySize / 2;

        // Clamp the sprite's position
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
