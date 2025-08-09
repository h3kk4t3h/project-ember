using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerStats : CharacterStats
{
    [SerializeField] private GameObject playerHUDPrefab;

    [SerializeField] private ClassSO classData;
    private float baseHealth;
    private float baseMana;
    private float manaRegen;
    private float spellPower; 

    // Armor
    [SerializeField] private float baseArmor = 50f;
    [SerializeField] private float armorRegenRate = 10f; 
    [SerializeField] private float armorRegenDelay = 2.5f; 
    [SerializeField, Tooltip("If damage > this value ignore armor and deal full damage to health")]
    private float armorBreakThreshold = 40f;
    private float currentArmor;
    private float lastDamageTime;
    private bool isArmorRegenerating = false;

    public float currentHealth { get; private set; }
    public float currentMana { get; private set; }
    public float CurrentArmor => currentArmor;
    public float maxArmor => baseArmor;
    public int currentXP { get; private set; }
    public int currentLevel { get; private set; } = 1;
    public int xpToNextLevel { get; private set; } = 100;
    public int skillPoints { get; private set; } = 0;
    public float maxHealth => baseHealth;
    public float maxMana => baseMana;
    public float SpellPower => spellPower;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;
    public event Action<float, float> OnArmorChanged;
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
        if (classData is MageClassSO mage)
        {
            baseMana = mage.baseMana;
            spellPower = mage.spellPower;
            manaRegen = mage.manaRegenRate;
        }
        else
        {
            baseMana = 0f;
            spellPower = 0f;
            manaRegen = 0f;
        }

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentArmor = maxArmor;
        currentXP = 0;
        currentLevel = 1;
        xpToNextLevel = 100;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);
        OnArmorChanged?.Invoke(currentArmor, maxArmor);
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
        ChangeMana(Mathf.FloorToInt(manaRegen));
        if (!isArmorRegenerating && Time.time - lastDamageTime >= armorRegenDelay && currentArmor < maxArmor)
        {
            isArmorRegenerating = true;
        }
        if (isArmorRegenerating && currentArmor < maxArmor)
        {
            RegenerateArmor();
        }
        if (isArmorRegenerating && currentArmor >= maxArmor)
        {
            isArmorRegenerating = false;
        }
    }

    private void RegenerateArmor()
    {
        float prevArmor = currentArmor;
        currentArmor = Mathf.Clamp(currentArmor + armorRegenRate, 0, maxArmor);
        if (currentArmor != prevArmor)
            OnArmorChanged?.Invoke(currentArmor, maxArmor);
    }

    public bool UseMana(float amount)
    {
        if (currentMana < amount) return false;
        ChangeMana(-amount);
        return true;
    }

    public override void TakeDamage(float amount)
    {
        // Stop armor regen on taking damage
        isArmorRegenerating = false;
        if (amount > armorBreakThreshold)
        {
            ChangeHealth(-amount);
            if (currentHealth <= 0) Die();
        }
        else
        {
            float damageLeft = amount;
            if (currentArmor > 0)
            {
                float absorbed = Mathf.Min(currentArmor, damageLeft);
                currentArmor -= absorbed;
                damageLeft -= absorbed;
                OnArmorChanged?.Invoke(currentArmor, maxArmor);
            }
            if (damageLeft > 0)
            {
                ChangeHealth(-damageLeft);
                if (currentHealth <= 0) Die();
            }
        }
        lastDamageTime = Time.time;
        CombatTextSpawner.Instance.ShowWorldText(
            $"−{amount}", Color.red, transform.position + Vector3.up * 1.8f);
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
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
        if (classData is MageClassSO mage)
        {
            baseMana += 10;
            spellPower += 5;
            manaRegen += 1f;
        }
        else
        {
            baseMana += 5;
            manaRegen += 0.5f;
        }
        baseArmor += 5f;
        currentArmor = maxArmor;
        OnArmorChanged?.Invoke(currentArmor, maxArmor);
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

    private void ChangeHealth(float delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void ChangeMana(float delta)
    {
        currentMana = Mathf.Clamp(currentMana + delta, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    // Health pots
    public void Heal(float amount)
    {
        ChangeHealth(amount);
    }

    public override void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("Player has died");
    }

    public void SetClassSO(ClassSO classSO)
    {
        classData = classSO;
      
    }
}