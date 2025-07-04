using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private MageClassSO classData; // Only serialize, not public

    public int currentHealth { get; private set; }
    public int currentMana { get; private set; }
    public int maxHealth => classData.baseHealth;
    public int maxMana => classData.baseMana;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnManaChanged;
    public event Action OnDeath;

    [SerializeField] private float regenInterval = 1f; // Expose regen interval in Inspector

    void Start()
    {
        if (classData == null)
        {
            Debug.LogError("PlayerStats: classData is not assigned!");
            enabled = false;
            return;
        }
        //Init
        currentHealth = maxHealth;
        currentMana = maxMana;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);

        // Regen loop
        InvokeRepeating(nameof(RegenTick), regenInterval, regenInterval);
    }

    // Regenerate Health and Mana
    void RegenTick()
    {
        ChangeHealth(Mathf.FloorToInt(classData.healthRegen));
        ChangeMana(Mathf.FloorToInt(classData.manaRegen));
    }

    // Use Mana
    public bool UseMana(int amount)
    {
        if (currentMana < amount) return false;
        {
            ChangeMana(-amount);
            return true;
        }
    }

    // Take Damage
    public void TakeDamage(int amount)
    {
        ChangeHealth(-amount);
        if (currentHealth <= 0) Die(); // Trigger death if health reaches 0
    }

    // Change Health
    private void ChangeHealth(int delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth); // Ensure health does not exceed max or drop below 0
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify listeners of health change
    }

    // Change Mana
    private void ChangeMana(int delta)
    {
        currentMana = Mathf.Clamp(currentMana + delta, 0, maxMana); // Ensure mana does not exceed max or drop below 0
        OnManaChanged?.Invoke(currentMana, maxMana); // Notify listeners of mana change
    }

    // Handle Player Death
    private void Die()
    {
        OnDeath?.Invoke(); // Notify listeners of death

        // Add respawn?

        // For debug
        Debug.Log("Player has died");
    }
}
