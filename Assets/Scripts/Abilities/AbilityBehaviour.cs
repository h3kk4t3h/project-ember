using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBehaviour : MonoBehaviour
{
    //General Ability Variables
    public Dictionary<Collider, int> targetsHit = new Dictionary<Collider, int>();

    //General Ability Functions
    public abstract IEnumerator SpawnAbility(Vector3 currentPosition, Vector3 hitPoint);
    public abstract AbilitySO GetAbilitySO();
    public abstract void ApplyAbilityDamage(GameObject target);

}
