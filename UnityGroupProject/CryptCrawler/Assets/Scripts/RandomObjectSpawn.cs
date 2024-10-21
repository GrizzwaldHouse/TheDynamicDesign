
using UnityEngine;
using System.Collections; // Importing the System.Collections namespace for using collections like IEnumerator
using System.Collections.Generic; // Importing the System.Collections.Generic namespace for using generic collections like List
using UnityEngine; // Importing the UnityEngine namespace for Unity-specific functionality

// Class responsible for spawning objects in waves and also handling random object spawning
public class ObjectSpawner : MonoBehaviour
{
    // List of spawn points where objects can be instantiated
    [SerializeField]
    private List<Transform> spawnPoints; // List of spawn points

    // The object that will be spawned
    [SerializeField]
    private GameObject spawnObject; // The object to spawn

    // Time interval between each object spawn
    [SerializeField]
    private float spawnInterval; // Time interval between spawns

    // Number of objects to spawn in each wave
    [SerializeField]
    private int objectsPerWave; // Number of objects to spawn in each wave

    // A specific spawn point for random object spawning
    [SerializeField]
    private Transform randomSpawnPoint; // The spawn point for random object spawning

    // The object to spawn when the trigger is activated
    [SerializeField]
    private GameObject randomSpawnObject; // The object to spawn on trigger

    // Start is called before the first frame update
    private void Start()
    {
        // Start the coroutine to spawn waves of objects
        StartCoroutine(SpawnWaves());
    }

    // Coroutine to handle the spawning of objects in waves
    private IEnumerator SpawnWaves()
    {
        // Infinite loop to continuously spawn waves
        while (true)
        {
            // Loop to spawn a specified number of objects in each wave
            for (int i = 0; i < objectsPerWave; i++)
            {
                // Call the method to spawn an object
                SpawnObject();
                // Wait for the specified interval before spawning the next object
                yield return new WaitForSeconds(spawnInterval);
            }
            // Wait for a longer interval before starting the next wave
            yield return new WaitForSeconds(spawnInterval * 2); // Delay between waves
        }
    }

    // Method to spawn an object at a random spawn point
    private void SpawnObject()
    {
        // Randomly select a spawn point from the list
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        // Instantiate the spawn object at the selected spawn point's position and rotation
        Instantiate(spawnObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
    }

    // Method called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            // Instantiate the random spawn object at the specified spawn point's position and rotation
            Instantiate(randomSpawnObject, randomSpawnPoint.position, randomSpawnPoint.rotation);
        }
    }
}
//public class RandomObjectSpawn : MonoBehaviour


//{
//    [SerializeField] private Transform SpawnPoint;
//    [SerializeField] private GameObject SpawnObject;
//    private void OnTriggerEnter()
//    {
//        Instantiate(SpawnObject, SpawnPoint.position, SpawnPoint.rotation);
//    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Check if the player has entered the trap
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        SpawnRandomObject();
    //    }
    //}
    //private void SpawnRandomObject()
    //{

    //    int randomIndex = Random.Range(0, RandomObjects.Length);
    //    Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
    //    Instantiate(RandomObjects[randomIndex], transform.position, Quaternion.identity);
    //}

