using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    public SpawnerSO spawnerConfig;
    public int waveNumber = 1;

    void Awake() {
        if (cameraController == null) {
            cameraController = GetComponent<CameraController>();
        }

        if (PlayerController.InstanceTransform != null) {
            SetCameraTarget(PlayerController.InstanceTransform);
        }
    }

    void Start()
    {
        spawnerConfig.spawnerNests = GameObject.FindGameObjectsWithTag("Spawner"); ;
    }

    private void Update()
    {
        spawnerConfig.enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (spawnerConfig.enemyCount == 0)
        {
            StartCoroutine(spawnerConfig.SpawnTimer());
            spawnerConfig.SpawnWave(waveNumber);
            waveNumber++;
        }
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