<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBehaviour : MonoBehaviour
{
    //General Ability Variables
    public Dictionary<Collider, int> targetsHit = new Dictionary<Collider, int>();
    public CharacterStats userStats;

    /// <summary>
    /// Play the SFX for this ability by index (0-3)
    /// </summary>
    protected void PlayAbilitySfx(int index)
    {
        AudioManager.PlayAbilitySfxStatic(index);
    }

    //General Ability Functions
    public abstract IEnumerator SpawnAbility(Vector3 currentPosition, Vector3 hitPoint);
    public abstract AbilitySO GetAbilitySO();
    public abstract float CalculateAbilityDamage();
    public abstract void ApplyAbilityDamage(GameObject target);
}
||||||| 49382df
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBehaviour : MonoBehaviour
{
    //General Ability Variables
    public Dictionary<Collider, int> targetsHit = new Dictionary<Collider, int>();
    public CharacterStats userStats;

    //General Ability Functions
    public abstract IEnumerator SpawnAbility(Vector3 currentPosition, Vector3 hitPoint);
    public abstract AbilitySO GetAbilitySO();
    public abstract float CalculateAbilityDamage();
    public abstract void ApplyAbilityDamage(GameObject target);

}
>>>>>>> origin/tech-demo
