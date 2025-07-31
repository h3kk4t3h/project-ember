using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Scriptable Objects/AbilitySO")]
public abstract class AbilitySO : ScriptableObject
{
    [Header("Base Attributes")]
    public int baseDamage = 0;
    public float fireRate = 0.0f;
    public float lifeTime = 0;
    public bool cooldownFlag = false;
    public float cooldown = 0;
}
