using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWaveSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Transform> spawnPoints; // List of spawn points

    [SerializeField]
    private GameObject spawnObject; // The object to spawn

    [SerializeField]
    private float spawnInterval ; // Time interval between spawns

    [SerializeField]
    private int objectsPerWave ; // Number of objects to spawn in each wave

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < objectsPerWave; i++)
            {
                SpawnObject();
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return new WaitForSeconds(spawnInterval * 2); // Delay between waves
        }
    }

    private void SpawnObject()
    {
        // Randomly select a spawn point from the list
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(spawnObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
    }
}
