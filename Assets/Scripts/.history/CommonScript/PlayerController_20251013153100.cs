using UnityEngine;

// This script lets the player move toward the mouse pointer smoothly,
// stop at a certain distance, slow down near the target,
// and control the flashlight direction using WASD.
public class PlayerController : MonoBehaviour
{
    #region Variables

    // Movement settings
    public float moveSpeed = 3f;          // Maximum movement speed
    public float stopDistance = 0.1f;     // Minimum distance from pointer before stopping (prevents jitter)
    public float slowDownDistance = 1f;   // Distance from pointer where slowing down begins

    // Whether the player is currently allowed to move
    public bool canMove = false;

    // Defines which layers are treated as "obstacles" for collision checks
    public LayerMask obstacleLayers;

    // Reference to the Rigidbody2D component for smooth physics-based movement
    private Rigidbody2D rigidBody;

    // Smoothed target position (lerped version)
    private Vector2 targetPosition;

    // Raw mouse target position (updated instantly each frame)
    private Vector2 rawTargetPosition;

    // Reference to flashlight Transform (assigned in Inspector)
    public Transform flashlightTransform;

    // The current facing direction of the flashlight (defaults to pointing up)
    private Vector2 flashlightDirection = Vector2.up;

    #endregion


    #region Built-In Functions

    // Called once at the start of the game
    private void Start()
    {
        // Cache the Rigidbody2D component
        rigidBody = GetComponent<Rigidbody2D>();

        // Makes Rigidbody2D interpolate between frames for smooth movement visuals
        rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Initialize target positions to current player position
        targetPosition = rigidBody.position;
        rawTargetPosition = targetPosition;
    }

    // Called every frame (good for reading input and camera updates)
    private void Update()
    {
        // If player movement is disabled, stop processing
        if (!canMove) return;

        // ----------------------------
        // Update target position (mouse)
        // ----------------------------

        // Convert mouse position from screen space to world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Store it as a 2D position (ignore Z)
        rawTargetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // ----------------------------
        // Handle flashlight direction (WASD)
        // ----------------------------
        Vector2 inputDir = Vector2.zero;

        // Determine which direction key is pressed
        if (Input.GetKey(KeyCode.W)) inputDir = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) inputDir = Vector2.down;
        else if (Input.GetKey(KeyCode.A)) inputDir = Vector2.left;
        else if (Input.GetKey(KeyCode.D)) inputDir = Vector2.right;

        // If a direction was pressed, update flashlight rotation and position
        if (inputDir != Vector2.zero)
        {
            flashlightDirection = inputDir;
            UpdateFlashlightDirection();
        }
    }

    // Called on a fixed time step — used for physics calculations
    private void FixedUpdate()
    {
        // Smoothly interpolate the target position to make mouse movement smoother
        float smoothingSpeed = 10f;
        targetPosition = Vector2.Lerp(targetPosition, rawTargetPosition, smoothingSpeed * Time.fixedDeltaTime);

        // Current player position
        Vector2 currentPos = rigidBody.position;

        // Calculate direction vector and distance to target
        Vector2 direction = targetPosition - currentPos;
        float distance = direction.magnitude;

        // If the player is far enough away from the target, move toward it
        if (distance > stopDistance)
        {
            direction.Normalize(); // Ensure direction length = 1

            // Determine movement speed — full until near the target, then slow down
            float speed = moveSpeed;

            // Start slowing down when within "slowDownDistance"
            if (distance < slowDownDistance)
            {
                // Linearly reduce speed between slowDownDistance → stopDistance
                speed = Mathf.Lerp(0, moveSpeed, (distance - stopDistance) / (slowDownDistance - stopDistance));

                // Ensure speed never goes below 0 or above max speed
                speed = Mathf.Clamp(speed, 0, moveSpeed);
            }

            // Cast a small ray forward to check if an obstacle is blocking the way
            RaycastHit2D hit = Physics2D.Raycast(
                currentPos,                               // Start from player
                direction,                                // Move direction
                speed * Time.fixedDeltaTime + 0.05f,      // How far ahead to check
                obstacleLayers                            // Only check specific layers
            );

            // If the ray didn't hit anything, move normally
            if (hit.collider == null)
            {
                // Calculate the new position
                Vector2 newPos = currentPos + direction * speed * Time.fixedDeltaTime;

                // Move the Rigidbody to that position (smooth physics-safe movement)
                rigidBody.MovePosition(newPos);
            }
            // If hit something, do nothing (prevents walking through walls)
        }
    }

    // Rotates and repositions the flashlight based on direction keys
    void UpdateFlashlightDirection()
    {
        // Calculate rotation angle (convert direction vector → degrees)
        float angle = Mathf.Atan2(flashlightDirection.y, flashlightDirection.x) * Mathf.Rad2Deg;

        // Apply rotation to flashlight so it faces the correct direction
        flashlightTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Determine position offset for flashlight (so it’s held in front of player)
        Vector3 offset = Vector3.zero;
        float offsetDistance = 4.0f; // How far the flashlight sits from the player

        // Adjust offset based on facing direction
        if (flashlightDirection == Vector2.up)
            offset = new Vector3(0, offsetDistance, 0);
        else if (flashlightDirection == Vector2.down)
            offset = new Vector3(0, -offsetDistance, 0);
        else if (flashlightDirection == Vector2.left)
            offset = new Vector3(-offsetDistance, 0, 0);
        else if (flashlightDirection == Vector2.right)
            offset = new Vector3(offsetDistance, 0, 0);

        // Apply local offset (relative to player)
        flashlightTransform.localPosition = offset;
    }

    #endregion
}
