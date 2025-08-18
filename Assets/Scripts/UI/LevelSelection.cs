<<<<<<< HEAD
// LevelSelectionUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button loadButton;
    public Button closeButton;

    [Header("Config")]
    public int arenaSceneIndex = 2;

    [Header("References")]
    public GameObject playerHUD;
    public GameObject interactionUI;

    void OnEnable()
    {
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() => {
            AudioManager.Instance.PlaySfx(SfxType.Button);
            LoadArena();
        });

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            AudioManager.Instance.PlaySfx(SfxType.Button);
            ClosePanel();
        });
    }

    private void LoadArena()
    {
        Debug.Log("Load button clicked!");
        SceneManager.LoadScene(arenaSceneIndex);
    }

    private void ClosePanel()
    {
        Debug.Log("Close button clicked!");
        if (playerHUD != null)
            playerHUD.SetActive(true);
  
        if (interactionUI != null)
            interactionUI.SetActive(true);

        gameObject.SetActive(false);
    }
}
||||||| 49382df
=======
// LevelSelectionUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button loadButton;
    public Button closeButton;

    [Header("Config")]
    public int arenaSceneIndex = 2;

    [Header("References")]
    public GameObject playerHUD;
    public GameObject interactionUI;

    void OnEnable()
    {
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(LoadArena);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void LoadArena()
    {
        Debug.Log("Load button clicked!");
        SceneManager.LoadScene(arenaSceneIndex);
    }

    private void ClosePanel()
    {
        Debug.Log("Close button clicked!");
        if (playerHUD != null)
            playerHUD.SetActive(true);
  
        if (interactionUI != null)
            interactionUI.SetActive(true);

        gameObject.SetActive(false);
    }
}
>>>>>>> origin/tech-demo
