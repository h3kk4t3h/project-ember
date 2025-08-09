using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviour : AbilityBehaviour
{
    // Projectile Ability specific Variables
    public ProjectileSO projectileData;
    private float spawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Projectile Movement
        HommingMovement(projectileData.homming);
        ProjectileMovement();

        //Projectile States
        LifeTimeCheck(spawnTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(projectileData.targetTags.ToString()))
        {
            ApplyAbilityDamage(other.gameObject);
            Destroy(gameObject);
        }
    }


    //ABILITY GENERAL FUNCTIONS

    //Spawn Ability - Shoot Projectile
    public override IEnumerator SpawnAbility(Vector3 userCurrentPosition, Vector3 hitPoint)
    {

            //Spawning projectiles
            for (int burstCount = 0; burstCount < projectileData.burst; burstCount++)
            {
                for (int multiCount = 0; multiCount < projectileData.numberOfProjectiles; multiCount++)
                {
                    //Setting Spawning Position & Rotation according Burst & MultiProjectiles
                    Vector3 spawnPosition = GetAbilitySpawnPosition(userCurrentPosition, hitPoint, burstCount, multiCount);
                    Quaternion spawnRotation = GetAbilitySpawnRotation(userCurrentPosition, hitPoint, multiCount);

                    Instantiate(this.gameObject, spawnPosition, spawnRotation);

                }
            }
        return null;
    }

    public override AbilitySO GetAbilitySO()
    {
        return projectileData;
    }

    public float CalculateAbilityDamage()
    {
        return projectileData.baseDamage;
    }

    public override void ApplyAbilityDamage(GameObject target)
    {
        target.GetComponent<CharacterStats>().TakeDamage(CalculateAbilityDamage());
    }


    //PROJECTILE SPECIFIC FUNCTIONS
    void ProjectileMovement()
    {
        //Standard direction of Movement
        Vector3 direction = Vector3.forward;

        transform.Translate(direction * projectileData.baseSpeed * Time.deltaTime);
    }

    private void HommingMovement(bool hommingBool)
    {
        //Add homming correction if enabled
        if (hommingBool)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, projectileData.hommingRadius);

            float angle = 0;
            float distance = 0;
            Collider enemyTargeted = null;


            //Find closest target enemy
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.gameObject.CompareTag(projectileData.targetTags.ToString()))
                {
                    float enemyDistance = (new Vector3((enemy.transform.position.x - transform.position.x), 0, (enemy.transform.position.z - transform.position.z))).magnitude;
                    if (distance > enemyDistance || distance == 0)
                    {
                        distance = enemyDistance;
                        enemyTargeted = enemy;
                    }
                }

            }

            //Calculate angle between the direction and the target
            if (enemyTargeted != null)
            {
                angle = Vector3.SignedAngle(transform.TransformDirection(Vector3.forward), (new Vector3((enemyTargeted.transform.position.x - transform.position.x), 0, (enemyTargeted.transform.position.z - transform.position.z))), Vector3.up);

                ////Check if the angle correction is higher than the curving speed
                if (Mathf.Abs(angle) > projectileData.hommingTurnRate)
                {

                    angle = projectileData.hommingTurnRate * (angle / Mathf.Abs(angle));

                }

                //Correct rotate object according to the angle
                transform.Rotate(Vector3.up, angle);
            }
        }
    }

    
    private void LifeTimeCheck(float spawnTime)
    {
        if(Time.time - spawnTime >= projectileData.lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetAbilitySpawnPosition(Vector3 userCurrentPosition, Vector3 hitPoint, int burstCount, int multiCount)
    {
        //Calculate Spawn Position with burst iteration in mind
        Vector3 localSpawnPosition = ((hitPoint - userCurrentPosition).normalized) * (1 + projectileData.distanceBetweenBurst * burstCount);

        //Rotate Spawn Position according Multi projectile rotation
        localSpawnPosition = Quaternion.AngleAxis(-projectileData.spreadBetweenProjectiles * (((float)(projectileData.numberOfProjectiles - 1) / 2) - multiCount), Vector3.up) * localSpawnPosition;
        return localSpawnPosition + userCurrentPosition;
    }

    public Quaternion GetAbilitySpawnRotation(Vector3 userCurrentPosition, Vector3 hitPoint, int multiCount)
    {
        //Calculate Spawn Rotation
        Quaternion spawnRotation = Quaternion.LookRotation((hitPoint - userCurrentPosition).normalized, Vector3.up);

        //Rotate Spawn Rotation according Multi projectile rotation
        spawnRotation = Quaternion.AngleAxis(-projectileData.spreadBetweenProjectiles * (((float)(projectileData.numberOfProjectiles - 1) / 2) - multiCount), Vector3.up) * spawnRotation;

        return spawnRotation;
    }


}
