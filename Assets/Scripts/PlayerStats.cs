using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private MageClassSO classData; 
    public int currentHealth { get; private set; }
    public int currentMana { get; private set; }
    public int currentXP { get; private set; }
    public int currentLevel { get; private set; } = 1;
    public int xpToNextLevel { get; private set; } = 100;
    public int maxHealth => classData.baseHealth;
    public int maxMana => classData.baseMana;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnManaChanged;
    public event Action<int, int> OnXPChanged; // currentXP, xpToNextLevel
    public event Action<int> OnLevelUp; // new level
    public event Action OnDeath;

    [SerializeField] private float regenInterval = 1f; // Expose regen interval in Inspector

    void Start()
    {
        if (classData == null)
        {
            Debug.LogError("PlayerStats: classData not assigned");
            enabled = false;
            return;
        }
        //Init
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentXP = 0;
        currentLevel = 1;
        xpToNextLevel = 100;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

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

    // Gain XP and handle level up
    public void GainXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.2f);

        // Increase base stats on level up
        classData.baseHealth += 10; // Increase health per level
        classData.baseMana += 5;    // Increase mana per level
        classData.healthRegen += 0.2f;
        classData.manaRegen += 0.5f;

        // Restore health and mana to new max
        currentHealth = maxHealth;
        currentMana = maxMana;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);

        OnLevelUp?.Invoke(currentLevel);
        // Other levelling rewards?
    }

    // Change Health
    private void ChangeHealth(int delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth); // Make sure health doesn't exceed max or drop below 0
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify listeners of health change
    }

    // Change Mana
    private void ChangeMana(int delta)
    {
        currentMana = Mathf.Clamp(currentMana + delta, 0, maxMana); // Make sure mana doesn't exceed max or drop below 0
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
