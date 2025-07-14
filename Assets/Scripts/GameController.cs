using UnityEngine;

public class GameController : MonoBehaviour
{
    public event System.Action<Transform> OnPlayerSpawned;

    void Start()
    {
        CameraController cameraController = FindFirstObjectByType<CameraController>();
        if (cameraController != null)
        {
            cameraController.SetTarget(transform);
        }
        OnPlayerSpawned?.Invoke(transform);
    }
}