using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyConfigSO configInstance;

    private NavMeshAgent agent;
    private Transform player;
    private float lastAttack;

    enum State { Idle, Chase, Attack }
    State state = State.Idle;

    private void Awake()
    {
        configInstance = GetComponent<EnemyStats>().GetEnemyConfigSOInstance();
        agent = GetComponent<NavMeshAgent>();

        if (PlayerController.InstanceTransform != null) {
            SetPlayerTarget(PlayerController.InstanceTransform);
        }
            ApplyConfig();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerSpawned += SetPlayerTarget;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerSpawned -= SetPlayerTarget;
    }

    private void SetPlayerTarget(Transform playerTransform)
    {
        if (playerTransform != null) {
            player = playerTransform;
        }
    }

    void ApplyConfig()
    {
        agent.speed = configInstance.speed;
        agent.acceleration = configInstance.acceleration;
        agent.stoppingDistance = configInstance.stoppingDistance;
    }

    void Update()
    {

        float dist = Vector3.Distance(transform.position, player.position);
        switch (state)
        {
            case State.Idle:
                if (dist <= configInstance.sightRange) state = State.Chase;
                Debug.Log("Idle");
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                Debug.Log("Chasing");
                if (dist <= configInstance.attackRange) state = State.Attack;
                else if (dist > configInstance.sightRange) state = State.Idle;
                break;

            case State.Attack:
                agent.ResetPath();
                FacePlayer();
                Debug.Log("Attacking");
                if (Time.time >= lastAttack + configInstance.attackCooldown)
                {
                    player.GetComponent<PlayerStats>()?.TakeDamage(configInstance.damage);
                    lastAttack = Time.time;
                }

                if (dist > configInstance.attackRange + 0.5f) state = State.Chase;
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