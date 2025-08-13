using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab; // prefab com InventoryItem + Image + Text

    // Adiciona 1 item (overload)
    public bool AddItem(ItemSO item)
    {
        return AddItem(item, 1);
    }

   //Add logic
    public bool AddItem(ItemSO item, int amount)
    {
        if (item == null || amount <= 0) return false;

        int remaining = amount;

        // 1) Se empilhável, tenta adicionar em stacks existentes
        if (item.stackable)
        {
            for (int i = 0; i < inventorySlots.Length && remaining > 0; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.transform.childCount > 0)
                {
                    InventoryItem existing = slot.GetComponentInChildren<InventoryItem>();
                    if (existing != null && existing.item == item && existing.count < item.maxStack)
                    {
                        remaining = existing.AddAmount(remaining);
                    }
                }
            }
        }

        // if slot full create items in empty slots
        for (int i = 0; i < inventorySlots.Length && remaining > 0; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.transform.childCount == 0)
            {
                int spawnAmount = item.stackable ? Mathf.Min(item.maxStack, remaining) : 1;
                SpawnNewItem(item, slot, spawnAmount);
                remaining -= spawnAmount;
            }
        }

        return remaining == 0;
    }

    void SpawnNewItem(ItemSO item, InventorySlot slot, int amount = 1)
    {
        GameObject newItemObject = Instantiate(InventoryItemPrefab, slot.transform);
        newItemObject.transform.localPosition = Vector3.zero;

        InventoryItem inventoryItem = newItemObject.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            inventoryItem.InitialiseItem(item, amount);
            inventoryItem.parentAfterDrag = slot.transform;
        }
        else
        {
            Debug.LogWarning("InventoryItem component missing on InventoryItemPrefab.");
        }
    }
}
