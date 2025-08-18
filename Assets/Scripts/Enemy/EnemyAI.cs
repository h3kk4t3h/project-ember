<<<<<<< HEAD
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyConfigSO configInstance;

    //Enemy General Variables
    private NavMeshAgent agent;
    private Transform player;
    private float lastAttack;

    //Abilities Associated Variables
    private Vector3 currentPosition;
    private Vector3 hitPoint;
    public GameObject ability;
    public AbilitySO abilityData;
    private float timeLastSpawn = 0;

    //Enemy State Vaiables
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

        abilityData = ability.GetComponent<AbilityBehaviour>().GetAbilitySO();
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
        //Calculate Targeted Position and Rotation for Projectile
        currentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = new Vector3(player.position.x, 1, player.position.z); ;

        float dist = Vector3.Distance(transform.position, player.position);
        switch (state)
        {
            case State.Idle:
                if (dist <= configInstance.sightRange) state = State.Chase;

                break;

            case State.Chase:
                agent.SetDestination(player.position);

                if (dist <= configInstance.attackRange) state = State.Attack;
                else if (dist > configInstance.sightRange) state = State.Idle;
                break;

            case State.Attack:
                agent.ResetPath();
                FacePlayer();

                UseAbility();

                if (dist > configInstance.attackRange * 0.5f) state = State.Chase;
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

    //ABILITY FUNCTIONS
    private void UseAbility()
    {

        //Cooldown Ability
        if (Time.time - timeLastSpawn >= abilityData.cooldown && abilityData.cooldownFlag)
        {
            StartCoroutine(ability.GetComponent<AbilityBehaviour>().SpawnAbility(currentPosition, hitPoint));
            timeLastSpawn = Time.time;
        }

        //Fire Rate Ability
        if (Time.time - timeLastSpawn >= 1 / abilityData.fireRate && !(abilityData.cooldownFlag))
        {
            StartCoroutine(ability.GetComponent<AbilityBehaviour>().SpawnAbility(currentPosition, hitPoint));
            timeLastSpawn = Time.time;
        }

    }
}
||||||| 49382df
=======
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyConfigSO configInstance;

    //Enemy General Variables
    private NavMeshAgent agent;
    private Transform player;
    private float lastAttack;

    //Abilities Associated Variables
    private Vector3 currentPosition;
    private Vector3 hitPoint;
    public GameObject ability;
    public AbilitySO abilityData;
    private float timeLastSpawn = 0;

    //Enemy State Vaiables
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

        abilityData = ability.GetComponent<AbilityBehaviour>().GetAbilitySO();
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
        //Calculate Targeted Position and Rotation for Projectile
        currentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = new Vector3(player.position.x, 1, player.position.z); ;

        float dist = Vector3.Distance(transform.position, player.position);
        switch (state)
        {
            case State.Idle:
                if (dist <= configInstance.sightRange) state = State.Chase;

                break;

            case State.Chase:
                agent.SetDestination(player.position);

                if (dist <= configInstance.attackRange) state = State.Attack;
                else if (dist > configInstance.sightRange) state = State.Idle;
                break;

            case State.Attack:
                agent.ResetPath();
                FacePlayer();

                UseAbility();

                if (dist > configInstance.attackRange * 0.5f) state = State.Chase;
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

    //ABILITY FUNCTIONS
    private void UseAbility()
    {

        //Cooldown Ability
        if (Time.time - timeLastSpawn >= abilityData.cooldown && abilityData.cooldownFlag)
        {
            StartCoroutine(ability.GetComponent<AbilityBehaviour>().SpawnAbility(currentPosition, hitPoint));
            timeLastSpawn = Time.time;
        }

        //Fire Rate Ability
        if (Time.time - timeLastSpawn >= 1 / abilityData.fireRate && !(abilityData.cooldownFlag))
        {
            StartCoroutine(ability.GetComponent<AbilityBehaviour>().SpawnAbility(currentPosition, hitPoint));
            timeLastSpawn = Time.time;
        }

    }
}
>>>>>>> origin/tech-demo
