using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    [Header("Toggles and Buttons")]
    [SerializeField] private Toggle warriorToggle;
    [SerializeField] private Toggle mageToggle;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    public static event Action OnNewGameStarted; // For AudioManager

    void Start()
    {
        warriorToggle.onValueChanged.AddListener(isOn => OnClassToggled(warriorToggle, mageToggle, isOn));
        mageToggle.onValueChanged.AddListener(isOn => OnClassToggled(mageToggle,warriorToggle, isOn));

        SetupStartButton();
        SetupExitButton();
    }

    private void OnClassToggled(Toggle changed, Toggle other, bool isOn)
    {
        if (isOn)
        {
            other.interactable = false;
            startButton.gameObject.SetActive(true);
        }
        else
        {
            other.interactable = true;
            startButton.gameObject.SetActive(false);
        }
    }

    private void SetupStartButton()
    {
        if (startButton == null)
            return;

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() =>
        {
            OnNewGameStarted?.Invoke(); // For AudioManager
            SceneManager.LoadScene(1);
        });
    }

    private void SetupExitButton()
    {
        if (exitButton == null)
            return;

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
