using UnityEngine;

[CreateAssetMenu(fileName = "MageClassSO", menuName = "Game/Player Classes/Mage")]
public class MageClassSO : ScriptableObject
{

    [Header("Base Attributes")]
    public int baseHealth = 100;
    public int baseMana = 200;

    [Header("Regen Rates (per sec)")]
    public float healthRegen = 1f;
    public float manaRegen = 5f;

    [Header("Other Stats")]
    public float movementSpeed = 5f;

}
