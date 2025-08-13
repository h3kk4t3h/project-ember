using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    [SerializeField] private EquipSlot[] equipSlots;
    private EquipmentSO[] currentBySlot;

    private void Awake()
    {
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats não encontrado na cena.");
            enabled = false;
            return;
        }

        if (equipSlots == null || equipSlots.Length == 0)
        {
            Debug.LogError("Nenhum EquipSlot atribuído.");
            enabled = false;
            return;
        }

        currentBySlot = new EquipmentSO[equipSlots.Length];

        for (int i = 0; i < equipSlots.Length; i++)
        {
            var slot = equipSlots[i];
            if (slot == null) continue;

            if (slot.slotIndex < 0) slot.slotIndex = i;

            int idx = i;

            slot.OnEquip += (eq, s) =>
            {
                if (eq == null) return;

                // remove antigo se houver
                if (currentBySlot[idx] != null && currentBySlot[idx] != eq)
                {
                    playerStats.RemoveEquipmentBonus(currentBySlot[idx]);
                }

                currentBySlot[idx] = eq;
                playerStats.AddEquipmentBonus(eq);
            };

            slot.OnUnequip += (eq, s) =>
            {
                if (currentBySlot[idx] != null)
                {
                    playerStats.RemoveEquipmentBonus(currentBySlot[idx]);
                    currentBySlot[idx] = null;
                }
                else if (eq != null)
                {
                    playerStats.RemoveEquipmentBonus(eq);
                }
            };
        }
    }
}
