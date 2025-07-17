using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public PlayerStats stats;
    public Button yesButton;

    [Header("UI Sliders")]
    public Slider healthFill;
    public Slider manaFill;
    public Slider xpFill;

    [Header("UI Text")]
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI xpText;

    void Start()
    {
        if (stats == null)
        {
            Debug.LogError("PlayerHUDController: stats not assigned", this);
            enabled = false;
            return;
        }
        stats.OnHealthChanged += UpdateHealth;
        stats.OnManaChanged += UpdateMana;
        stats.OnXPChanged += UpdateXP;
        stats.OnLevelUp += UpdateLevel;

        healthFill.minValue = 0;
        healthFill.maxValue = stats.maxHealth;

        manaFill.minValue = 0;
        manaFill.maxValue = stats.maxMana;

        xpFill.minValue = 0;
        xpFill.maxValue = stats.xpToNextLevel;

        UpdateHealth(stats.currentHealth, stats.maxHealth);
        UpdateMana(stats.currentMana, stats.maxMana);
        UpdateXP(stats.currentXP, stats.xpToNextLevel);
        UpdateLevel(stats.currentLevel);
        SetupYesButton();
    }
    void OnDestroy()
    {
        if (stats != null)
        {
            stats.OnHealthChanged -= UpdateHealth;
            stats.OnManaChanged -= UpdateMana;
            stats.OnXPChanged -= UpdateXP;
            stats.OnLevelUp -= UpdateLevel;
        }
    }

    void UpdateHealth(int current, int max)
    {
        // In case maxHealth changes on level up
        healthFill.maxValue = max;
        healthFill.value = current;
        if (healthText != null)
            healthText.text = $"{current} / {max}";
    }

    void UpdateMana(int current, int max)
    {
        manaFill.maxValue = max;
        manaFill.value = current;
        if (manaText != null)
            manaText.text = $"{current} / {max}";
    }

    void UpdateXP(int currentXP, int xpToNext)
    {
        Debug.Log($"XP UI updated: {currentXP}/{xpToNext}");
        xpFill.maxValue = xpToNext;
        xpFill.value = currentXP;
        if (xpText != null)
            xpText.text = $"{currentXP} / {xpToNext}";
    }

    void UpdateLevel(int level)
    {
        levelLabel.text = level.ToString();
    }
    
    private void SetupYesButton()
    {
        if (yesButton == null)
        {
            Debug.LogError("HUD: YesButton not assigned", this);
            return;
        }

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() =>
    {
    #if UNITY_EDITOR
            // Stop play mode in the editor
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            // Quit application in build
            Application.Quit();
    #endif
        });
}

}
