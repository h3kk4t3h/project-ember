using UnityEngine;

[CreateAssetMenu(fileName = "MageClassSO", menuName = "Game/Player Classes/Mage")]
public class MageClassSO : ClassSO
{
    [Header("Mage Attributes")]
    public float baseMana = 100f; 
    public float spellPower = 20f; 
    public float manaRegenRate = 5f; 
                                     

}
