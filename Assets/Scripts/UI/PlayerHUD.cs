using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public PlayerStats stats;
    public Button yesExitButton;
    public Button yesHubExitButton;
    public Button yesMainMenuExitButton;
    public Button mainMenuButton;
    public GameObject deathPanel;
    public Button retreatButton;


    [Header("UI Sliders")]
    public Slider healthFill;
    public Slider manaFill;
    public Slider xpFill;

    [Header("UI Text")]
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelUPText;
    public TextMeshProUGUI skillPointsText;

    public float levelMessageDuration = 3f;

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
        stats.OnDeath += ShowDeathPanel;

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

        levelUPText.gameObject.SetActive(false);
        skillPointsText.gameObject.SetActive(false);

        SetupYesExitButton();
        SetupYesHubExitButton();
        SetupYesMainMenuExitButton();
        HideMainMenuButton();
        SetupRetreatButton();
    }



    void OnDestroy()
    {
        if (stats != null)
        {
            stats.OnHealthChanged -= UpdateHealth;
            stats.OnManaChanged -= UpdateMana;
            stats.OnXPChanged -= UpdateXP;
            stats.OnLevelUp -= UpdateLevel;
            stats.OnDeath -= ShowDeathPanel;
        }
    }

    void UpdateHealth(int current, int max)
    {
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
        xpFill.maxValue = xpToNext;
        xpFill.value = currentXP;
        if (xpText != null)
            xpText.text = $"{currentXP} / {xpToNext}";
    }

    void UpdateLevel(int level)
    {
        levelLabel.text = level.ToString();
        StartCoroutine(ShowLevelUpMessages(level));
    }

    private IEnumerator ShowLevelUpMessages(int level)
    {
        levelUPText.text = $"Level Up! You are now level {level}!";
        skillPointsText.text = $"Skill Points: {stats.skillPoints}";

        levelUPText.gameObject.SetActive(true);
        skillPointsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(levelMessageDuration);

        levelUPText.gameObject.SetActive(false);
        skillPointsText.gameObject.SetActive(false);
    }

    private void SetupYesExitButton()
    {
        if (yesExitButton == null)
            return;

        yesExitButton.onClick.RemoveAllListeners();
        yesExitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
    }

    private void HideMainMenuButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false);
        }
    }

    private void SetupYesHubExitButton()
    {
        if (yesHubExitButton == null)
            return;

        yesHubExitButton.onClick.RemoveAllListeners();
        yesHubExitButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    private void SetupYesMainMenuExitButton()
    {
        if (yesMainMenuExitButton == null)
            return;

        yesMainMenuExitButton.onClick.RemoveAllListeners();
        yesMainMenuExitButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private void ShowDeathPanel()
    {
    if (deathPanel != null)
        deathPanel.SetActive(true);
    }
    
    private void SetupRetreatButton()
    {
        if (retreatButton != null)
        {
            retreatButton.onClick.RemoveAllListeners();
            retreatButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        }
    }
}
