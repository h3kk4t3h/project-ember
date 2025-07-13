using UnityEngine;

public class HubSpawner : MonoBehaviour
{
    [SerializeField] private GameObject magePrefab;
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        string className = PlayerPrefs.GetString("SelectedClass", magePrefab.name);
        GameObject prefab = className == warriorPrefab.name ? warriorPrefab : magePrefab;
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
