using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OptionsHUD : MonoBehaviour
{
    [Header("Buttons")]
    public Button optionsButton;
    public Button backHubButton;
    public Button closeButton;
    public GameObject optionsPanel;
    public Button yesExitButton;
    public Button yesHubExitButton;
    public Button yesMainMenuExitButton;

    void Start()
    {
        SetupYesExitButton();
        SetupYesHubExitButton();
        SetupYesMainMenuExitButton();
        HideBackHubButton();
    }

    void Update()
    {
        PauseGameOnOptionsPanel();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (optionsPanel.activeSelf)
                closeButton.onClick.Invoke();
            else
                optionsButton.onClick.Invoke();
        }
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

    private void PauseGameOnOptionsPanel()
    {

        if (optionsButton != null && optionsButton.gameObject.activeSelf)
        {
            Time.timeScale = 1f; 
        }
        else
        {
            Time.timeScale = 0f; 
        }
    }
    private void HideBackHubButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && backHubButton != null)
        {
            backHubButton.gameObject.SetActive(false);
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
}
