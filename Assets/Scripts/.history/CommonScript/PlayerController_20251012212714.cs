using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public float moveSpeed = 3f; //player's speed towards pointer
    public float stopDistance = 0.1f; //min distance from pointer at which player stops -- prevents jittering upon reaching the pointer
    public float slowDownDistance = 1f; // start slowing down when closer than this

    public bool canMove = false;

    public LayerMask obstacleLayers;  // A LayerMask that determines which layers are considered "obstacles" -- only objects on these layers block movement
    private Rigidbody2D rigidBody; //reference to RigidBody2D component -- used for movement and collisions

    private Vector2 targetPosition; // Smoothed target position that player moves toward
    private Vector2 rawTargetPosition; // Raw mouse position updated every frame

    public Transform flashlightTransform; // Assign in inspector
    private Vector2 flashlightDirection = Vector2.up; // Default facing up

    #endregion

    #region Built In Functions

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        targetPosition = rigidBody.position;
        rawTargetPosition = targetPosition;
    }

    // Called every frame (not tied to physics)
    // Used for reading input, such as mouse position
    private void Update()
    {
        if (!canMove) return; // stop everything until it's true
        {
            // Capture raw mouse position every frame
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rawTargetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // WASD flashlight control
            Vector2 inputDir = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) inputDir = Vector2.up;
            else if (Input.GetKey(KeyCode.S)) inputDir = Vector2.down;
            else if (Input.GetKey(KeyCode.A)) inputDir = Vector2.left;
            else if (Input.GetKey(KeyCode.D)) inputDir = Vector2.right;

            if (inputDir != Vector2.zero)
            {
                flashlightDirection = inputDir;
                UpdateFlashlightDirection();
            }
        }
    }


    // Called every fixed framerate frame (default 50fps)
    // Used for physics-based updates like movement
    private void FixedUpdate()
    {
        float smoothingSpeed = 10f;
        targetPosition = Vector2.Lerp(targetPosition, rawTargetPosition, smoothingSpeed * Time.fixedDeltaTime);

        Vector2 currentPos = rigidBody.position;
        Vector2 direction = targetPosition - currentPos;
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();

            // Calculate speed scaling: full speed until slowDownDistance, then slow down smoothly to zero at stopDistance
            float speed = moveSpeed;

            if (distance < slowDownDistance)
            {
                speed = Mathf.Lerp(0, moveSpeed, (distance - stopDistance) / (slowDownDistance - stopDistance));
                // speed will be 0 at stopDistance, full moveSpeed at slowDownDistance
                speed = Mathf.Clamp(speed, 0, moveSpeed);
            }

            RaycastHit2D hit = Physics2D.Raycast(
                currentPos,
                direction,
                speed * Time.fixedDeltaTime + 0.05f,
                obstacleLayers
            );

            if (hit.collider == null)
            {
                Vector2 newPos = currentPos + direction * speed * Time.fixedDeltaTime;
                rigidBody.MovePosition(newPos);
            }
        }
    }

    void UpdateFlashlightDirection()
    {
        float angle = Mathf.Atan2(flashlightDirection.y, flashlightDirection.x) * Mathf.Rad2Deg;
        flashlightTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Set position offset depending on direction
        Vector3 offset = Vector3.zero;
        float offsetDistance = 4.0f; // How far flashlight is held from player center

        if (flashlightDirection == Vector2.up)
            offset = new Vector3(0, offsetDistance, 0);
        else if (flashlightDirection == Vector2.down)
            offset = new Vector3(0, -offsetDistance, 0);
        else if (flashlightDirection == Vector2.left)
            offset = new Vector3(-offsetDistance, 0, 0);
        else if (flashlightDirection == Vector2.right)
            offset = new Vector3(offsetDistance, 0, 0);

        // Update flashlight position relative to player (this transform)
        flashlightTransform.localPosition = offset;
    }


    #endregion
}
