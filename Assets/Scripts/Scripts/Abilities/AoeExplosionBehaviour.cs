using System.Collections;
using UnityEngine;

public class AoeExplosionBehaviour : AbilityBehaviour
{
    // AOE Explosion Ability specific Variables
    public AoeExplosionSO AOEData;
    private float spawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        //Variation of Scale during activation
        ChangeAbilityScale();

        //AOE Explosion States
        LifeTimeCheck(spawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(AOEData.targetTags.ToString()))
        {
            if (!targetsHit.ContainsKey(other))
            {
                targetsHit.Add(other, 1);
                ApplyAbilityDamage(other.gameObject);
            }
        }
    }

    //ABILITY GENERAL FUNCTIONS

    //Spawn Ability - AOE Explosion
    public override IEnumerator SpawnAbility(Vector3 userCurrentPosition, Vector3 hitPoint)
    {

        //Spawning AOE Explosion
        for (int multiCount = 1; multiCount <= AOEData.repetitionCount; multiCount++)
        {
            //Setting Spawning Position & Rotation
            Vector3 spawnPosition = GetAbilitySpawnPosition(hitPoint);
            Quaternion spawnRotation = GetAbilitySpawnRotation(userCurrentPosition, hitPoint);

            if(multiCount > 1)
            {
                yield return new WaitForSeconds(AOEData.repetitionDelay);
            }
            GameObject instance = Instantiate(this.gameObject, spawnPosition, spawnRotation);
            //Set Starting Scale with Multiple AOE in mind
            SetStartingScale(instance, multiCount);
        }
    }

    public override AbilitySO GetAbilitySO()
    {
        return AOEData;
    }

    public override float CalculateAbilityDamage()
    {
        float abilityDamage = AOEData.baseDamage;
        return abilityDamage;
    }

    public override void ApplyAbilityDamage(GameObject target)
    {
        target.GetComponent<CharacterStats>().TakeDamage(AOEData.baseDamage);
    }


    //AOE SPECIFIC FUNCTIONS
    private void LifeTimeCheck(float spawnTime)
    {
        if (Time.time - spawnTime >= AOEData.lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetAbilitySpawnPosition(Vector3 hitPoint)
    {
        //Calculate Spawn Position with reach in mind
        Vector3 localSpawnPosition = hitPoint;

        return localSpawnPosition;
    }

    public Quaternion GetAbilitySpawnRotation(Vector3 userCurrentPosition, Vector3 hitPoint)
    {
        //Calculate Spawn Rotation
        Quaternion spawnRotation = Quaternion.LookRotation((hitPoint - userCurrentPosition).normalized, Vector3.up);

        return spawnRotation;
    }

    public void SetStartingScale(GameObject instance, int repetitionCounter)
    {
        instance.transform.localScale *= (AOEData.radius + AOEData.repetitionScaleIncrease * (repetitionCounter - 1)) * AOEData.startingRadiusRelative;
    }

    public void ChangeAbilityScale()
    {
        Vector3 scale = transform.localScale * (1 + (1 - AOEData.startingRadiusRelative) / (AOEData.lifeTime * AOEData.startingRadiusRelative) * Time.deltaTime);

        transform.localScale = scale;
    }


}
