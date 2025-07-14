using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    public EnemyConfigSO config;
    private NavMeshAgent agent;
    private Transform player;
    private float lastAttack;

    enum State { Idle, Chase, Attack }
    State state = State.Idle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameController gameController = FindFirstObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.OnPlayerSpawned += SetPlayerTarget;
        }
        ApplyConfig();
    }

    private void SetPlayerTarget(Transform playerTransform)
    {
        player = playerTransform;
    }

    void ApplyConfig()
    {
        agent.speed = config.speed;
        agent.acceleration = config.acceleration;
        agent.stoppingDistance = config.stoppingDistance;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        switch (state)
        {
            case State.Idle:
                if (dist <= config.sightRange) state = State.Chase;
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                if (dist <= config.attackRange) state = State.Attack;
                else if (dist > config.sightRange) state = State.Idle;
                break;

            case State.Attack:
                agent.ResetPath();
                FacePlayer();
                if (Time.time >= lastAttack + config.attackCooldown)
                {
                    player.GetComponent<PlayerStats>()?.TakeDamage(config.damage);
                    lastAttack = Time.time;
                }
                if (dist > config.attackRange + 0.5f) state = State.Chase;
                break;
        }
    }

    void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir.magnitude > 0f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                10f * Time.deltaTime
            );
        }
    }
}
