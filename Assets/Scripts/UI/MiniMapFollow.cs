using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        var stats = FindFirstObjectByType<PlayerStats>();
        if (stats != null)
        {
            player = stats.transform;
        }
        else
        {
            Debug.LogError("MiniMapFollow: PlayerStats not found in the scene.");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
