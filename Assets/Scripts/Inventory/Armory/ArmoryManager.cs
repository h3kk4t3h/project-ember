using System.Collections.Generic;
using UnityEngine;

public class ArmoryManager : MonoBehaviour
{
    [Header("Armory slots")]
    [SerializeField] private ArmorySlot[] armorySlots;

    [Header("Equipment Item Prefab")]
    [SerializeField] private GameObject equipmentItemPrefab;

    private void Awake()
    {
        if (armorySlots == null) armorySlots = new ArmorySlot[0];
        for (int i = 0; i < armorySlots.Length; i++)
        {
            if (armorySlots[i] != null)
                armorySlots[i].Setup(i, this);
        }
    }

 
    //Add logic 
    public bool AddEquipment(EquipmentSO equipment)
    {
        if (equipment == null) return false;

        for (int i = 0; i < armorySlots.Length; i++)
        {
            var s = armorySlots[i];
            if (s == null) continue;
            if (s.IsEmpty())
            {
                SpawnEquipmentItemInSlot(equipment, s);
                return true;
            }
        }

        Debug.Log($"ArmoryManager: nenhum slot livre para {equipment.itemName}");
        return false;
    }

    public EquipmentItem SpawnEquipmentItemInSlot(EquipmentSO equipment, ArmorySlot slot)
    {
        if (equipment == null || slot == null)
        {
            Debug.LogWarning("ArmoryManager: equipamento ou slot é null.");
            return null;
        }

        if (equipmentItemPrefab == null)
        {
            Debug.LogError("ArmoryManager: equipmentItemPrefab não atribuído!");
            return null;
        }

        GameObject go = Instantiate(equipmentItemPrefab, slot.transform);
        go.transform.localPosition = Vector3.zero;
        var ei = go.GetComponent<EquipmentItem>();
        if (ei == null)
        {
            Debug.LogError("equipmentItemPrefab não tem componente EquipmentItem.");
            Destroy(go);
            return null;
        }

        ei.Initialise(equipment);
        ei.parentAfterDrag = slot.transform;
        return ei;
    }

    /// <summary>
    /// Remove e devolve a instância do slot; não destrói o objeto.
    /// </summary>
    public EquipmentItem RemoveFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= armorySlots.Length) return null;
        return armorySlots[slotIndex].RemoveItem();
    }

    // opcional: expõe os slots
    public ArmorySlot[] GetSlots() => armorySlots;
}
