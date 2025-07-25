using UnityEngine;

public abstract class AbilityBehaviour : MonoBehaviour
{
    //General Ability Variables
    public abstract void SpawnAbility(Vector3 playerCurrentPosition, Vector3 hitPoint);
    public abstract AbilitySO GetAbilitySO();
}
