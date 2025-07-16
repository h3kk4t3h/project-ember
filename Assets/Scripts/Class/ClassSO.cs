using UnityEngine;

[CreateAssetMenu(fileName = "ClassSO", menuName = "Game/Player Classes/Base")]
public class ClassSO : ScriptableObject
{
    [Header("Base Attributes")]
    public int baseHealth = 100;
    public int baseMana = 100;
    [Header("Regen Rates (per sec)")]
    public float healthRegen = 1f;
    public float manaRegen = 1f;
    [Header("Other Stats")]
    public float movementSpeed = 5f;
}
