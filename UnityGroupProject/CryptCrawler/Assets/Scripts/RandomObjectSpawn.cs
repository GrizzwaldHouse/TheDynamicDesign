using UnityEngine; // Importing UnityEngine for Unity-specific functionality
using System.Collections.Generic; // Importing for using List
using UnityEngine.UI;
using TMPro;
using System.Collections;
// Class responsible for spawning objects at designated points
public class ObjectSpawner : MonoBehaviour
{
    // List of spawn points where objects can be instantiated
    [SerializeField] private List<Transform> spawnPoints; // List of spawn points

    // The object that will be spawned
    [SerializeField] private GameObject spawnObject; // The object to spawn

    // List to keep track of all spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Reference to the quest manager
    //  private IQuestManager questManager;

    // Variable to hold the last spawned object
    private GameObject lastSpawnedObject;

    // Maximum number of destroyed objects before the spawner is disabled
    [SerializeField] int maxDestroyedCount; // Set this to your desired limit
    private int currentDestroyedCount; // Counter for destroyed objects
    private bool canSpawn = true;
    [SerializeField] AudioSource audioSource; // AudioSource to play sound effects
    [SerializeField] AudioClip spawnSound; // Sound effect to play when an object is spawned
    [SerializeField] float soundDuration;
    [SerializeField] TMP_Text remainingObjectsText; // Text to display remaining objects
    [SerializeField] TMP_Text destroyedCountText; // Text to display destroyed count


    private void Start()
    {
        // Initialize the quest manager if needed
        // questManager = FindObjectOfType<QuestManager>(); // Assuming IQuestManager is a component in the scene
        // GameManager.instance.AddSpawner(this); // Assuming GameManager is a singleton
    }

    // Method to spawn an object at a random spawn point
    private void SpawnObject()
    {
        if (!canSpawn || currentDestroyedCount >= maxDestroyedCount)
        {
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0 || spawnObject == null)
        {
            Debug.LogError("Spawn points or spawn object is not set.");
            return;
        }

        // Randomly select a spawn point from the list
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instantiate the spawn object at the selected spawn point's position and rotation
        GameObject spawnedObject = Instantiate(spawnObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        if (audioSource != null && spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
            StartCoroutine(StopSoundAfterDuration(soundDuration));
        }
        // Add the spawned object to the list for tracking
        spawnedObjects.Add(spawnedObject);
        lastSpawnedObject = spawnedObject; // Update the last spawned object reference
        DisableSpawner();
        canSpawn = false;
        UpdateUI();
    }
    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop(); // Stop playing the sound effect
    }
    // Method called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnObject(); // Spawn an object when the player enters the trigger
                           //  questManager?.NotifyObjectSpawned(this); // Notify the QuestManager that an object has been spawned
        }
    }

    // Method to disable the spawner's trigger
    public void DisableSpawner()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // Disable the collider
                                      // questManager?.RemoveSpawner(this);
        }
    }

    // Method to destroy all spawned objects
    public void DestroyAllSpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj); // Destroy the spawned object
                currentDestroyedCount++; // Increment the destroyed count
            }
        }

        // Disable the spawner if the max destroyed count is reached
        if (currentDestroyedCount >= maxDestroyedCount)
        {
            DisableSpawner();
        }

        // Clear the list after destroying the objects
        spawnedObjects.Clear(); // Reset the list for future spawns
        UpdateUI();
    }

    // Method to handle an object being destroyed
    public void ObjectDestroyed(GameObject destroyedObject)
    {
        if (spawnedObjects.Contains(destroyedObject))
        {
            spawnedObjects.Remove(destroyedObject); // Remove the destroyed object from the list
            currentDestroyedCount++; // Increment the destroyed count

            // Disable the spawner if the max destroyed count is reached
            if (currentDestroyedCount >= maxDestroyedCount)
            {
                DisableSpawner();
                DestroyAllSpawnedObjects();
            }
            else
            {
                EnableSpawner();
                canSpawn = true; // Re-enable spawning after a short delay
            }
            UpdateUI();
        }
    }

    // Method to get the last spawned object
    public GameObject GetLastSpawnedObject()
    {
        return lastSpawnedObject; // Return the last spawned object
    }

    // Method to reset the spawner
    public void ResetSpawner()
    {
        DestroyAllSpawnedObjects(); // Clear all spawned objects
        currentDestroyedCount = 0; // Reset the destroyed count
        EnableSpawner(); // Optionally re-enable the spawner if needed
        canSpawn = true; // Re-enable spawning after a short delay
        UpdateUI();
    }

    // Method to enable the spawner's trigger
    public void EnableSpawner()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true; // Enable the collider
        }
    }
    private void UpdateUI()
    {

        if (remainingObjectsText != null)
        {
            remainingObjectsText.text = "Remaining Objects: " + (maxDestroyedCount - currentDestroyedCount);
        }
        if (destroyedCountText != null)
        {
            destroyedCountText.text = "Destroyed Objects: " + currentDestroyedCount;
        }
    }
}