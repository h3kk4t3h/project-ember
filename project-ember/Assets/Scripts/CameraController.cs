using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // The player to follow
    [SerializeField] private Vector3 offset = new Vector3(-10, 10, -10); // Isometric offset
    [SerializeField] private float smoothSpeed = 5f; // Camera follow smoothing

    private void LateUpdate()
    {
        if (target == null)
        {
            // Try to find the player by tag if not assigned
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                return;
        }
        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;
        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        // Optionally, look at the player
        transform.LookAt(target);
    }
}
