using UnityEngine;
using System.Collections;

public class SporadicLaserPointer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed of the laser pointer
    public float directionChangeInterval = 1f; // Time in seconds between direction changes
    public float respawnDelay = 2f; // Time before the laser reappears after being hit
    public float edgeMarginPixels = 30f; // Margin from the screen edge in pixels

    private Vector2 screenBounds; // Screen bounds in world space
    private Vector2 moveDirection; // Current movement direction
    private bool isVisible = true; // Track visibility
    private float edgeMargin; // Margin in world units

    private void Start()
    {
        // Get the screen bounds in world space
        Camera mainCamera = Camera.main;
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);

        // Convert edge margin from pixels to world units
        edgeMargin = mainCamera.ScreenToWorldPoint(new Vector3(edgeMarginPixels, 0, 0)).x - mainCamera.ScreenToWorldPoint(Vector3.zero).x;

        // Start the direction change coroutine
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        if (isVisible)
        {
            // Move the laser pointer in the current direction
            transform.Translate(moveDirection * speed * Time.deltaTime);

            // Keep the laser within the screen bounds and adjust direction if clamped
            AdjustDirectionIfClamped();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.name);

        // Only react to collisions with the player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Laser hit the player!");
            StartCoroutine(HandleLaserHit());
        }
    }

    private IEnumerator HandleLaserHit()
    {
        // Make the laser disappear
        isVisible = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Reposition the laser randomly near the screen edges
        RepositionLaser();

        // Make the laser reappear
        isVisible = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            if (isVisible)
            {
                // Choose a random direction
                float angle = Random.Range(0f, 360f);
                moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            }

            // Wait for the next direction change
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void AdjustDirectionIfClamped()
    {
        // Check and clamp the laser pointer's position within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float clampedY = Mathf.Clamp(transform.position.y, 0 + edgeMargin, screenBounds.y - edgeMargin);

        if (clampedX != transform.position.x || clampedY != transform.position.y)
        {
            // Laser was clamped, so adjust the movement direction
            Vector2 center = new Vector2(0, screenBounds.y / 2); // Approximate screen center
            moveDirection = ((Vector2)transform.position - center).normalized * -1; // Redirect away from the screen edge
        }

        // Apply the clamped position
        transform.position = new Vector2(clampedX, clampedY);
    }

    private void RepositionLaser()
    {
        // Reposition the laser to a random position near the edges of the screen
        float newX = Random.Range(-screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float newY = Random.Range(0 + edgeMargin, screenBounds.y - edgeMargin);
        transform.position = new Vector2(newX, newY);
    }
}
