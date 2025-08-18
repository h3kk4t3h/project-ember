<<<<<<< HEAD
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-5, 10, -9); // Isometric offset
    [SerializeField] private float smoothSpeed = 100f; // Camera follow smoothing

    private Transform target;

    private void LateUpdate()
    {
        if (target != null) {
            MoveCamera();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void MoveCamera()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
||||||| 49382df
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-10, 10, -10); // Isometric offset
    [SerializeField] private float smoothSpeed = 5f; // Camera follow smoothing

    private Transform target;

    private void LateUpdate()
    {
        if (target != null)
            MoveCamera();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void MoveCamera()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
=======
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-5, 10, -9); // Isometric offset
    [SerializeField] private float smoothSpeed = 100f; // Camera follow smoothing

    private Transform target;

    private void LateUpdate()
    {
        if (target != null) {
            MoveCamera();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void MoveCamera()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
>>>>>>> origin/tech-demo
