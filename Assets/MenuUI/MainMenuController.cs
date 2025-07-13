using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Document")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject magePrefab;
    [SerializeField] private GameObject warriorPrefab;

    private VisualElement root;
    private VisualElement mainMenu;
    private VisualElement newGameScreen;
    private VisualElement loadGameScreen;
    private VisualElement optionsScreen;
    private VisualElement loadingScreen;

    private Toggle mageToggle;
    private Toggle warriorToggle;
    private Button confirmClassButton;

    private Slider volumeSlider;
    private Toggle fullscreenToggle;

    private ProgressBar loadingProgressBar;

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;

        mainMenu = root.Q<VisualElement>("MainMenu");
        newGameScreen = root.Q<VisualElement>("NewGameScreen");
        loadGameScreen = root.Q<VisualElement>("LoadGameScreen");
        optionsScreen = root.Q<VisualElement>("OptionsScreen");
        loadingScreen = root.Q<VisualElement>("LoadingScreen");

        mageToggle = root.Q<Toggle>("MageToggle");
        warriorToggle = root.Q<Toggle>("WarriorToggle");
        confirmClassButton = root.Q<Button>("ConfirmClassButton");

        volumeSlider = root.Q<Slider>("VolumeSlider");
        fullscreenToggle = root.Q<Toggle>("FullscreenToggle");

        loadingProgressBar = root.Q<ProgressBar>("LoadingProgressBar");

        MainMenuButtons();
        NewGameLogic();
        OptionsLogic();
    }

    private void MainMenuButtons()
    {
        root.Q<Button>("NewGameButton").clicked += () => ShowScreen(newGameScreen);
        root.Q<Button>("LoadGameButton").clicked += () => ShowScreen(loadGameScreen);
        root.Q<Button>("OptionsButton").clicked += () => ShowScreen(optionsScreen);
        root.Q<Button>("ExitButton").clicked += () =>
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        };


        root.Q<Button>("CancelClassButton").clicked += () => ShowScreen(mainMenu);
        root.Q<Button>("BackToMainMenuButton").clicked += () => ShowScreen(mainMenu);
        root.Q<Button>("BackFromOptionsButton").clicked += () => ShowScreen(mainMenu);
    }

    private void NewGameLogic()
    {
        confirmClassButton.clicked += () =>
        {
            if (!mageToggle.value && !warriorToggle.value)
                return;

            string className = mageToggle.value ? magePrefab.name : warriorPrefab.name;
            PlayerPrefs.SetString("SelectedClass", className);

            ShowScreen(loadingScreen);
            loadingProgressBar.value = 0;

            StartCoroutine(LoadSceneAsync(1));
        };
    }

    private void OptionsLogic()
    {
        volumeSlider.RegisterValueChangedCallback(evt =>
        {
            AudioListener.volume = evt.newValue;
        });

        fullscreenToggle.RegisterValueChangedCallback(evt =>
        {
            Screen.fullScreen = evt.newValue;
        });
    }

    private void ShowScreen(VisualElement screenToShow)
    {
        mainMenu.style.display = DisplayStyle.None;
        newGameScreen.style.display = DisplayStyle.None;
        loadGameScreen.style.display = DisplayStyle.None;
        optionsScreen.style.display = DisplayStyle.None;
        loadingScreen.style.display = DisplayStyle.None;

        screenToShow.style.display = DisplayStyle.Flex;
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingProgressBar.value = progress * 100f;
            yield return null;
        }

        loadingProgressBar.value = 100f;

        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
        Debug.Log($"Loading progress: {loadingProgressBar.value}");

    }

}
