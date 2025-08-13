using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSO", menuName = "Scriptable Objects/MeleeSO")]
public class MeleeSO : AbilitySO
{
    [Header("Reach Attributes")]
    public float reachMultiplier = 0;
    public float projectSpeed = 10;
    public float projectScaleIncrease = 1.5f;

    [Header("Melee Repeated Attack Attributes")]
    public int currentComboChain = 1;
    public int maxComboChain = 1;
    public float comboSizeScale = 0;
    public int multipleAttacks = 1;
    public float multipleAttacksSpread = 0;

}
