using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSO", menuName = "Scriptable Objects/MeleeSO")]
public class MeleeSO : AbilitySO
{
    [Header("Melee Number Attributes")]
    public int numberOfProjectiles = 1;
    public float spreadBetweenProjectiles = 0;
    public int burst = 1;
    public float distanceBetweenBurst = 0;

    [Header("Reach Attributes")]
    public float reachSpawn = 0;
    public float baseSpeed = 10;
}
