
using UnityEngine; // Importing UnityEngine for Unity-specific functionality
using System.Collections.Generic; // Importing for using List

// Class responsible for spawning objects at designated points
public class ObjectSpawner : MonoBehaviour
{
    // List of spawn points where objects can be instantiated
    [SerializeField]
    private List<Transform> spawnPoints; // List of spawn points

    // The object that will be spawned
    [SerializeField]
    private GameObject spawnObject; // The object to spawn

    // List to keep track of all spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();
    // Reference to the quest manager
    private IQuestManager questManager;
    private GameObject lastSpawnedObject; // Variable to hold the last spawned object
     // Maximum number of destroyed objects before the spawner is disabled
    [SerializeField]
    private int maxDestroyedCount ; // Set this to your desired limit
    private int currentDestroyedCount ; // Counter for destroyed objects
    private void Start()
    {
       // gamemanager.instance.AddSpawner(this); // Assuming GameManager is a singleton

    }
    // Method to spawn an object at a random spawn point
    private void SpawnObject()
    {
        // Randomly select a spawn point from the list
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instantiate the spawn object at the selected spawn point's position and rotation
        GameObject spawnedObject = Instantiate(spawnObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        // Add the spawned object to the list for tracking
        spawnedObjects.Add(spawnedObject);
        lastSpawnedObject = spawnedObject; // Update the last spawned object reference
    }

    // Method called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Spawn an object when the player enters the trigger
            SpawnObject();
            // Notify the QuestManager that an object has been spawned
            questManager?.NotifyObjectSpawned(this);
        }
    }

    // Method to disable the spawner's trigger
    public void DisableSpawner()
    {
        // Disable the collider to stop triggering
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // Disable the collider
        }
    }

    // Method to destroy all spawned objects
    public void DestroyAllSpawnedObjects()
    {
        // Iterate through the list of spawned objects
        foreach (var obj in spawnedObjects)
        {
            // Check if the object is not null
            if (obj != null)
            {
                Destroy(obj); // Destroy the spawned object
            }
        }
        // Clear the list after destroying the objects
        spawnedObjects.Clear(); // Reset the list for future spawns
    }

    public void ObjectDestroyed()
    {
     
    }

    // Method to get the last spawned object
    public GameObject GetLastSpawnedObject()
    {
        return lastSpawnedObject; // Return the last spawned object
    }

}