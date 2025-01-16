using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SporadicLaserPointer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Speed of the laser pointer
    public float directionChangeInterval = 1f; // Time in seconds between direction changes
    public float respawnDelay = 2f; // Time before the laser reappears after being hit

    private Vector2 screenBounds; // Screen bounds
    private Vector2 moveDirection; // Current movement direction
    private bool isVisible = true; // Track visibility
    private float edgeMargin = 10f; // Margin in world units from the screen edge

    private void Start()
    {
        // Get the screen bounds in world space
        Camera mainCamera = Camera.main;
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);

        // Convert the margin from pixels to world units
        edgeMargin = mainCamera.ScreenToWorldPoint(new Vector3(edgeMargin, 0, 0)).x - mainCamera.ScreenToWorldPoint(Vector3.zero).x;

        // Start the direction change coroutine
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        if (isVisible)
        {
            // Move the laser pointer in the current direction
            transform.Translate(moveDirection * speed * Time.deltaTime);

            // Keep the laser within 10 pixels of the edges
            ClampPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision object has a Rigidbody2D
        if (collision.attachedRigidbody != null)
        {
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

        // Reposition the laser randomly near the edges of the screen
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
            else
            {
                moveDirection = Vector2.zero; // Stop movement while invisible
            }

            // Wait for the next direction change
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void ClampPosition()
    {
        // Clamp the laser pointer's position to within 10 pixels of the screen edges
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float clampedY = Mathf.Clamp(transform.position.y, -screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);
        transform.position = new Vector2(clampedX, clampedY);
    }

    private void RepositionLaser()
    {
        // Reposition the laser to a random position within 10 pixels of the edges
        float newX = Random.Range(-screenBounds.x + edgeMargin, screenBounds.x - edgeMargin);
        float newY = Random.Range(-screenBounds.y + edgeMargin, screenBounds.y - edgeMargin);
        transform.position = new Vector2(newX, newY);
    }
}