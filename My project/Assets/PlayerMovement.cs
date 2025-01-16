using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Horizontal movement speed
    public float jumpForce = 10f; // Jumping force

    [Header("Audio Settings")]
    public AudioClip walkSound; // Sound to play while walking
    public float walkSoundVolume = 0.5f; // Volume of the walking sound

    private Rigidbody2D rb; // Reference to Rigidbody2D
    private float horizontalInput; // Player's horizontal input
    private bool isFacingRight = true; // Track the character's facing direction
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        // Get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // Jump when the player presses the jump button
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            Jump();
        }

        // Flip the character's sprite based on movement direction
        FlipCharacter();

        // Play or stop walking sound based on movement
        HandleMovementSound();
    }

    void FixedUpdate()
    {
        // Apply horizontal movement (don't change y-velocity)
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        // Add an upward force to the Rigidbody2D for jumping
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void FlipCharacter()
    {
        // Flip only when moving in the opposite direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Invert the facing direction
        isFacingRight = !isFacingRight;

        // Flip the character by changing the local scale (only x-axis)
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void HandleMovementSound()
    {
        // Play walking sound when the player is moving
        if (Mathf.Abs(horizontalInput) > 0 && !audioSource.isPlaying)
        {
            audioSource.clip = walkSound;
            audioSource.volume = walkSoundVolume;
            audioSource.loop = true;
            audioSource.Play();
        }
        // Stop walking sound when the player stops moving
        else if (Mathf.Abs(horizontalInput) == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
