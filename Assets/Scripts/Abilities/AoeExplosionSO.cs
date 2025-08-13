using UnityEngine;

[CreateAssetMenu(fileName = "AoeExplosionSO", menuName = "Scriptable Objects/AoeExplosionSO")]
public class AoeExplosionSO : AbilitySO
{
    [Header("AOE Attributes")]
    public float radius = 0;
    public float startingRadiusRelative = 0.5f;
    public float projectScaleIncrease = 1.5f;

    [Header("Multiple AOE Attributes")]
    public int repetitionCount = 1;
    public float repetitionDelay = 0;
    public float repetitionScaleIncrease = 0;
}
