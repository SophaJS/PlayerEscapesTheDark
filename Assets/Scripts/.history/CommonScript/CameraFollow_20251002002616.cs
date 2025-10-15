using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // player
    public float smoothSpeed = 0.125f; // how smooth the camera follows
    public Vector3 offset;      // offset so camera isn’t dead-centered

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 60);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
