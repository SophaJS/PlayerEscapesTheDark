using UnityEngine;

/// <summary>
/// This script smoothly follows a target (usually the player) with a slight delay and offset.
/// It prevents the camera from snapping directly to the player's position,
/// creating a smoother and more natural movement.
/// </summary>

public class CameraFollow : MonoBehaviour
{
    // The object the camera should follow — typically the player’s transform.
    public Transform target;

    // How quickly the camera catches up to the target.
    // Higher values = faster movement; lower = smoother and slower.
    public float smoothSpeed = 0.125f;

    // The camera’s positional offset relative to the target.
    // For example, (0, 3, -10) might place the camera above and behind the player.
    public Vector3 offset;

    // LateUpdate is used instead of Update so the camera moves AFTER the target has moved.
    // This ensures the camera always follows the player’s most up-to-date position.
    void LateUpdate()
    {
        // If no target is assigned (to avoid null reference errors), do nothing.
        if (target == null) return;

        // Calculate where we *want* the camera to be:
        // the target’s position plus the offset (e.g., slightly above/behind the player).
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate (lerp) from the current position to the desired position.
        // Multiplying by Time.deltaTime * 60 keeps the smoothing consistent across frame rates.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 60);

        // Apply the new position to the camera, but keep the original Z value.
        // This keeps the camera at a fixed distance in 2D or side-scrolling games.
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
