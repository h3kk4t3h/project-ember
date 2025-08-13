using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler
{
    public EquipSlotType accepts;    
    public int slotIndex = -1;       
    public bool hideOnEmpty = false; 

    public System.Action<EquipmentSO, int> OnEquip;     
    public System.Action<EquipmentSO, int> OnUnequip;   

    private EquipmentItem lastEquipped;

    public bool Accepts(EquipSlotType t) => t == accepts;
    public bool IsEmpty() => transform.childCount == 0;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null) return;

        var dragged = eventData.pointerDrag.GetComponent<EquipmentItem>();
        if (dragged == null) return;

        if (!Accepts(dragged.equipment.equipSlot)) return;

        // slot vazio
        if (IsEmpty())
        {
            dragged.parentAfterDrag = transform;
            dragged.transform.SetParent(transform, false);
            dragged.transform.localPosition = Vector3.zero;

            lastEquipped = dragged;
            OnEquip?.Invoke(dragged.equipment, slotIndex);
            return;
        }

        // swap
        var existingItem = transform.GetChild(0).GetComponent<EquipmentItem>();
        if (existingItem != null)
        {
            Transform draggedOriginalParent = dragged.parentAfterDrag;

            existingItem.transform.SetParent(draggedOriginalParent, false);
            existingItem.transform.localPosition = Vector3.zero;
            existingItem.parentAfterDrag = draggedOriginalParent;

            dragged.parentAfterDrag = transform;
            dragged.transform.SetParent(transform, false);
            dragged.transform.localPosition = Vector3.zero;

            OnUnequip?.Invoke(existingItem.equipment, slotIndex);
            lastEquipped = dragged;
            OnEquip?.Invoke(dragged.equipment, slotIndex);
        }
    }

    private void OnTransformChildrenChanged()
    {
        // se ficou vazio e havia item, remove bônus
        if (IsEmpty() && lastEquipped != null)
        {
            OnUnequip?.Invoke(lastEquipped.equipment, slotIndex);
            lastEquipped = null;
        }
    }

    public EquipmentItem RemoveItem()
    {
        if (IsEmpty()) return null;
        var child = transform.GetChild(0);
        var ei = child.GetComponent<EquipmentItem>();
        if (ei != null)
        {
            OnUnequip?.Invoke(ei.equipment, slotIndex);
            ei.transform.SetParent(null, true);
            ei.parentAfterDrag = null;
            lastEquipped = null;
            return ei;
        }
        return null;
    }
}
