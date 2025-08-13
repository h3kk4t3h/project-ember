using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehaviour : AbilityBehaviour
{
    // Melee Ability specific Variables
    public MeleeSO meleeData;
    private float spawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Melee Project Movement
        MeleeMovement();

        //Variation of Scale during Movement & Combo
        SetAbilitySpawnScale();

        //Melee Attack States
        LifeTimeCheck(spawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(meleeData.targetTags.ToString()))
        {
            if (!targetsHit.ContainsKey(other))
            {
                targetsHit.Add(other, 1);
                ApplyAbilityDamage(other.gameObject);
            }
        }
    }

    //ABILITY GENERAL FUNCTIONS

    //Spawn Ability - Melee Attack
    public override IEnumerator SpawnAbility(Vector3 userCurrentPosition, Vector3 hitPoint)
    {

        //Spawning Melee Attacks
        for (int multiCount = 0; multiCount < meleeData.multipleAttacks; multiCount++)
        {
            //Setting Spawning Position & Rotation according Burst & MultiProjectiles
            Vector3 spawnPosition = GetAbilitySpawnPosition(userCurrentPosition, hitPoint, multiCount);
            Quaternion spawnRotation = GetAbilitySpawnRotation(userCurrentPosition, hitPoint, multiCount);

            GameObject instance = Instantiate(this.gameObject, spawnPosition, spawnRotation);

            meleeData.currentComboChain++;
            if (meleeData.currentComboChain > meleeData.maxComboChain)
            {
                meleeData.currentComboChain = 1;
            }

        }

        yield return new WaitForSeconds(0);
    }

    public override AbilitySO GetAbilitySO()
    {
        return meleeData;
    }

    public override float CalculateAbilityDamage()
    {
        float abilityDamage = meleeData.baseDamage;
        return abilityDamage;
    }

    public override void ApplyAbilityDamage(GameObject target)
    {
        target.GetComponent<CharacterStats>().TakeDamage(meleeData.baseDamage);
    }


    //MELEE SPECIFIC FUNCTIONS
    void MeleeMovement()
    {
        //Standard direction of Movement
        Vector3 direction = Vector3.forward;

        transform.Translate(direction * meleeData.projectSpeed * Time.deltaTime);
    }

    private void LifeTimeCheck(float spawnTime)
    {
        if (Time.time - spawnTime >= meleeData.lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetAbilitySpawnPosition(Vector3 userCurrentPosition, Vector3 hitPoint, int multiCount)
    {
        //Calculate Spawn Position with reach in mind
        Vector3 localSpawnPosition = ((hitPoint - userCurrentPosition).normalized) * (1 + meleeData.reachMultiplier);

        //Rotate Spawn Position according Multiple Attack rotation
        localSpawnPosition = Quaternion.AngleAxis(-meleeData.multipleAttacksSpread * (((float)(meleeData.multipleAttacks - 1) / 2) - multiCount), Vector3.up) * localSpawnPosition;
        return localSpawnPosition + userCurrentPosition;
    }

    public Quaternion GetAbilitySpawnRotation(Vector3 userCurrentPosition, Vector3 hitPoint, int multiCount)
    {
        //Calculate Spawn Rotation
        Quaternion spawnRotation = Quaternion.LookRotation((hitPoint - userCurrentPosition).normalized, Vector3.up);

        //Rotate Spawn Rotation according Multiple Attack rotation
        spawnRotation = Quaternion.AngleAxis(-meleeData.multipleAttacksSpread * (((float)(meleeData.multipleAttacks - 1) / 2) - multiCount), Vector3.up) * spawnRotation;

        return spawnRotation;
    }

    public void SetAbilitySpawnScale()
    {
        Vector3 spawnScale = transform.localScale * (1 + (meleeData.projectScaleIncrease  + meleeData.comboSizeScale * (meleeData.currentComboChain - 1)) / meleeData.lifeTime * Time.deltaTime);

        transform.localScale = spawnScale;
    }
}
