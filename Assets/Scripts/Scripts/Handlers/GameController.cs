using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    public SpawnerSO spawnerConfig;
    public int waveNumber = 1;
    
    private Scene currenctScene;


    void Awake() {
        currenctScene = SceneManager.GetActiveScene();

        if (cameraController == null) {
            cameraController = GetComponent<CameraController>();
        }

        if (PlayerController.InstanceTransform != null) {
            SetCameraTarget(PlayerController.InstanceTransform);
        }
    }

    void Start() {
        if (SceneManager.Equals(currenctScene, SceneManager.GetSceneByName("Arena"))) {
            spawnerConfig.spawnerNests = GameObject.FindGameObjectsWithTag("Spawner");
            //Debug.Log("Finding Spawners");
        }
    }

    private void Update()
    {
        if (SceneManager.Equals(currenctScene, SceneManager.GetSceneByName("Arena")))
        {
            spawnerConfig.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            //Debug.Log("Fetching enemies!");


            if (spawnerConfig.enemyCount == 0 && spawnerConfig.spawnFlag)
            {
                StartCoroutine(spawnerConfig.SpawnTimer());
                spawnerConfig.SpawnWave(waveNumber);
                waveNumber++;
            }
        }
    }

    public IEnumerator WaitTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }

    private void OnEnable() {
        PlayerController.OnPlayerSpawned += SetCameraTarget;
    }

    private void OnDisable() {
        PlayerController.OnPlayerSpawned -= SetCameraTarget;
    }

    private void SetCameraTarget(Transform playerTransform) {
        cameraController.SetTarget(playerTransform);
    }

}