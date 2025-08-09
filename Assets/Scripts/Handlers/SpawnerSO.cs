using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerSO", menuName = "Scriptable Objects/SpawnerSO")]
public class SpawnerSO : ScriptableObject
{
    [Header("Spawn Attributes")]
    public GameObject[] enemies;
    public int[] waveSize;
    public float waveSpawnTimer;


    //Spawner Logistics
    public GameObject[] spawnerNests;
    public int enemyCount;
    public bool spawnFlag = true;

    public void SpawnWave(int waveNumber)
    {
        FindFirstObjectByType<EnemyCount>()?.SetWave(waveNumber);

        for (int i = 0; i < enemies.Length; i++)
        {
            SpawnEnemies(enemies[i], waveSize[i] * waveNumber);
        }
    }

    public void SpawnEnemies(GameObject enemyType, int enemyAmount)
    {
        spawnerNests = GameObject.FindGameObjectsWithTag("Spawner");

        while(enemyAmount > 0)
        {
            int spawNumberPerNest = enemyAmount / spawnerNests.Length;
            if(spawNumberPerNest == 0)
            {
                spawNumberPerNest = 1;
            }

            foreach (GameObject spawnerNest in spawnerNests)
            {
                for(int i = 0; i < spawNumberPerNest; i++)
                {
                    Instantiate(enemyType, GetRandomSpawnPosition(spawnerNest), enemyType.transform.rotation);
                    enemyAmount--;
                    if(enemyAmount == 0) 
                        return;
                }

            }
        }
    }

    Vector3 GetRandomSpawnPosition(GameObject spawnerNest)
    {
        Vector3 spawnPositionCenter = spawnerNest.transform.position;

        float x = Random.Range(-spawnerNest.GetComponent<BoxCollider>().size.x / 2, spawnerNest.GetComponent<BoxCollider>().size.x / 2);
        float z = Random.Range(-spawnerNest.GetComponent<BoxCollider>().size.z / 2, spawnerNest.GetComponent<BoxCollider>().size.z / 2);

        return spawnPositionCenter + new Vector3(x, 0, z);
    }

    public IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(waveSpawnTimer);
    }

}
