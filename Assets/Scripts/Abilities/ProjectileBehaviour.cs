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
        //Destroy(gameObject, projectileData.lifeTime);
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
        if (other.CompareTag("Enemy"))
        {
            ApplyAbilityDamage(other.gameObject);
            Destroy(gameObject);
        }
    }


    //ABILITY GENERAL FUNCTIONS

    //Spawn Ability - Shoot Projectile
    public override void SpawnAbility(Vector3 playerCurrentPosition, Vector3 hitPoint)
    {

            //Spawning projectiles
            for (int burstcount = 0; burstcount < projectileData.burst; burstcount++)
            {
                for (int multiCount = 0; multiCount < projectileData.numberOfProjectiles; multiCount++)
                {
                    //Setting Spawning Position & Rotation according Burst & MultiProjectiles
                    Vector3 spawnPosition = GetAbilitySpawnPosition(playerCurrentPosition, hitPoint, burstcount, multiCount);
                    Quaternion spawnRotation = GetAbilitySpawnRotation(playerCurrentPosition, hitPoint, multiCount);

                    Instantiate(this.gameObject, spawnPosition, spawnRotation);

                }
            }
    }

    public override AbilitySO GetAbilitySO()
    {
        return projectileData;
    }

    public override void ApplyAbilityDamage(GameObject enemy)
    {
        enemy.GetComponent<EnemyStats>().TakeDamage(projectileData.baseDamage);
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
                if (enemy.gameObject.CompareTag("Enemy"))
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

    public Vector3 GetAbilitySpawnPosition(Vector3 playerCurrentPosition, Vector3 hitPoint, int burstcount, int multiCount)
    {
        //Calculate Spawn Position with burst iteration in mind
        Vector3 localSpawnPosition = ((hitPoint - playerCurrentPosition).normalized) * (1 + projectileData.distanceBetweenBurst * burstcount);

        //Rotate Spawn position according Multi projectile rotation
        localSpawnPosition = Quaternion.AngleAxis(-projectileData.spreadBetweenProjectiles * (((float)(projectileData.numberOfProjectiles - 1) / 2) - multiCount), Vector3.up) * localSpawnPosition;
        return localSpawnPosition + playerCurrentPosition;
    }

    public Quaternion GetAbilitySpawnRotation(Vector3 playerCurrentPosition, Vector3 hitPoint, int multiCount)
    {
        //Calculate Spawn Rotation
        Quaternion spawnRotation = Quaternion.LookRotation((hitPoint - playerCurrentPosition).normalized, Vector3.up);

        //Rotate Spawn Rotation according Multi projectile rotation
        spawnRotation = Quaternion.AngleAxis(-projectileData.spreadBetweenProjectiles * (((float)(projectileData.numberOfProjectiles - 1) / 2) - multiCount), Vector3.up) * spawnRotation;

        return spawnRotation;
    }


}
