using UnityEngine;

public class InventoryTestSpawner : MonoBehaviour
{
    public ItemSO testItem; // arrasta aqui o ItemSO no Inspector
    public EquipmentSO testEquipment;
    public EquipmentSO testEquipment2;// opcional, se quiser testar EquipmentItem
    public int amount = 2;

    void Start()
    {
        // opcional: auto-spawn ao iniciar para testes
        SpawnTestItem();
        SpawnTestEquipment();
        SpawnTestEquipment2();
    }

    [ContextMenu("Spawn Test Item (Inspector)")]
    public void SpawnTestItem()
    {
        InventoryManager inv = FindFirstObjectByType<InventoryManager>();
        if (inv == null)
        {
            Debug.LogError("[InventoryTestSpawner] Não foi encontrado nenhum InventoryManager na cena.");
            return;
        }

        if (testItem == null)
        {
            Debug.LogError("[InventoryTestSpawner] TestItem não está atribuído no Inspector.");
            return;
        }

        bool result = inv.AddItem(testItem, amount);
        if (result)
            Debug.Log($"[InventoryTestSpawner] Item adicionado: {testItem.itemName} x{amount}");
        else
            Debug.LogWarning($"[InventoryTestSpawner] Não houve espaço no inventário para: {testItem.itemName} x{amount}");
    }

    public void SpawnTestEquipment()
    {
        ArmoryManager armory = FindFirstObjectByType<ArmoryManager>();
        armory.AddEquipment(testEquipment);
    }

    public void SpawnTestEquipment2()
    {
       ArmoryManager armory2 = FindFirstObjectByType<ArmoryManager>();
       armory2.AddEquipment(testEquipment2);
    }
}
