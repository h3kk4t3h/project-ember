<<<<<<< HEAD
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalInteraction : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject interactionUI;
    public GameObject levelSelectionUI;
    public GameObject playerHUD;

    private bool playerNearby;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (levelSelectionUI != null)
            levelSelectionUI.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenLevelSelection();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void OpenLevelSelection()
    {
        if (playerHUD != null) playerHUD.SetActive(false);
        if (interactionUI != null) interactionUI.SetActive(false);
        if (levelSelectionUI != null) levelSelectionUI.SetActive(true);
    }
}
||||||| 49382df
=======
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalInteraction : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject interactionUI;
    public GameObject levelSelectionUI;
    public GameObject playerHUD;

    private bool playerNearby;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (levelSelectionUI != null)
            levelSelectionUI.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenLevelSelection();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void OpenLevelSelection()
    {
        if (playerHUD != null) playerHUD.SetActive(false);
        if (interactionUI != null) interactionUI.SetActive(false);
        if (levelSelectionUI != null) levelSelectionUI.SetActive(true);
    }
}
>>>>>>> origin/tech-demo
