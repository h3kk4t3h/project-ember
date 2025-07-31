using UnityEngine;

public abstract class AbilityBehaviour : MonoBehaviour
{
    //General Ability Variables
    public abstract void SpawnAbility(Vector3 currentPosition, Vector3 hitPoint);
    public abstract AbilitySO GetAbilitySO();
    public abstract void ApplyAbilityDamage(GameObject target);
}
