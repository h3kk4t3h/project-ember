using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject playerHUDPrefab;

    [SerializeField] private ClassSO classData;
    private int baseHealth;
    private int baseMana;
    private float healthRegen;
    private float manaRegen;

    public int currentHealth { get; private set; }
    public int currentMana { get; private set; }
    public int currentXP { get; private set; }
    public int currentLevel { get; private set; } = 1;
    public int xpToNextLevel { get; private set; } = 100;
    public int skillPoints { get; private set; } = 0;
    public int maxHealth => baseHealth;
    public int maxMana => baseMana;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnManaChanged;
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLevelUp;
    public event Action OnDeath;

    [SerializeField] private float regenInterval = 1f;

    void Start()
    {
        if (classData == null)
        {
            Debug.LogError("PlayerStats: classData not assigned");
            enabled = false;
            return;
        }
        baseHealth = classData.baseHealth;
        baseMana = classData.baseMana;
        healthRegen = classData.healthRegen;
        manaRegen = classData.manaRegen;

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentXP = 0;
        currentLevel = 1;
        xpToNextLevel = 100;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

        InvokeRepeating(nameof(RegenTick), regenInterval, regenInterval);

        if (playerHUDPrefab != null)
        {
            GameObject hud = Instantiate(playerHUDPrefab);
            PlayerHUD hudScript = hud.GetComponent<PlayerHUD>();
            if (hudScript != null)
                hudScript.stats = this;
        }
        else
        {
            Debug.LogError("PlayerStats: esquecido de arrastar o playerHUDPrefab no Inspector!");
        }
    }

    void RegenTick()
    {
        ChangeHealth(Mathf.FloorToInt(healthRegen));
        ChangeMana(Mathf.FloorToInt(manaRegen));
    }

    public bool UseMana(int amount)
    {
        if (currentMana < amount) return false;
        ChangeMana(-amount);
        return true;
    }

    public void TakeDamage(int amount)
    {
        ChangeHealth(-amount);
        if (currentHealth <= 0) Die();

        // Spawn the “–200” text above the player
        CombatTextSpawner.Instance.ShowWorldText(
        $"−{amount}", Color.red, transform.position + Vector3.up * 1.8f);
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        // Spawn the “+50 XP” text above the player
        CombatTextSpawner.Instance.ShowWorldText(
        $"+{amount} XP", Color.yellow , transform.position + Vector3.up * 2.2f);

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

        baseHealth += 10;
        baseMana += 5;
        healthRegen += 0.2f;
        manaRegen += 0.5f;

        currentHealth = maxHealth;
        currentMana = maxMana;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);

        OnLevelUp?.Invoke(currentLevel);

        skillPoints += 1;

    }

    public bool SpendSkillPoints()
    {
        if (skillPoints > 0)
        {
            skillPoints--;
            return true;
        }
        return false;
    }

    private void ChangeHealth(int delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void ChangeMana(int delta)
    {
        currentMana = Mathf.Clamp(currentMana + delta, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("Player has died");
    }

    public void SetClassSO(ClassSO classSO)
    {
        classData = classSO;
        }
}