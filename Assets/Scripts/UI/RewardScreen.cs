using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RewardScreen : MonoBehaviour
{
    [Header("References")]
    public Button continueButton;

    [Header("UI Text")]
    public TextMeshProUGUI xpText;

    [Header("Reward Settings")]
    public int minXP = 100;
    public int maxXP = 300;

    private int rewardXP;


    void OnEnable()
    {
        rewardXP = Random.Range(minXP, maxXP + 1);
        GrantRewardXP();
        UpdateXPText();
        SetupContinueButton();
    }

    private void GrantRewardXP()
    {
        var player = FindFirstObjectByType<PlayerStats>();

        player.GainXP(rewardXP);
    }

    // Show “+N XP” on the panel
    private void UpdateXPText()
    {
        if (xpText == null)
        {
            Debug.LogError("RewardScreen: xpText not assigned", this);
            return;
        }

        xpText.text = $"+{rewardXP} XP";
    }

    private void SetupContinueButton()
    {
        if (continueButton == null)
        {
            Debug.LogError("RewardScreen: continueButton not assigned", this);
            return;
        }
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            AudioManager.Instance.PlaySfx(SfxType.Button);
            SceneManager.LoadScene(1);
        });
    }
}


