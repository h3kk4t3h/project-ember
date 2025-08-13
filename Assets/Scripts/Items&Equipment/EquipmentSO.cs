using UnityEngine;

public enum EquipSlotType
{
    WeaponMain,  
    WeaponOffhand, 
    Accessory,
    Head,
    Chest,
    Legs,
    Hands,
    Feet
}

[CreateAssetMenu(menuName = "Items/EquipmentSO", fileName = "NewEquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    [Header("Information")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Shop price")]
    public float shopBuyPrice = 0;
    public float shopSellPrice = 0;

    [Header("Slot")]
    public EquipSlotType equipSlot = EquipSlotType.Head;

    [Header("Stat bonuses")]
    public float bonusMaxHealth = 0; 
    public float bonusMaxMana = 0;  
    public float bonusArmor = 0f;  
    public float bonusSpellPower = 0f;
}
