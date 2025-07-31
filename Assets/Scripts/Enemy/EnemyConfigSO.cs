using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemy Config")]
public class EnemyConfigSO : ScriptableObject
{
    [Header("NavMesh Agent Settings")]
    public float speed = 3.5f;
    public float acceleration = 8f;
    public float stoppingDistance = 2f;

    [Header("Combat")]
    public float health = 100f;
    public float sightRange = 10f;
    public float attackRange = 2f;
    public int damage = 10;
    public float attackCooldown = 1.5f;
        
}
