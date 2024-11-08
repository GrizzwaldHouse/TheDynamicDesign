using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the triggering of projectile instantiation when an object enters a designated area.
public class TriggerProjectile : MonoBehaviour
{
    // Serialized fields to expose variables in the Unity Inspector for easy configuration
    [SerializeField] private List<GameObject> projectilePrefabs; // List of projectile prefab GameObjects to be instantiated
    [SerializeField] private List<Transform> spawnPoints; // List of Transform objects representing where projectiles will spawn
    [SerializeField] private float fireRate; // Time in seconds between each projectile firing

    private float nextFireTime; // Tracks the time when the next projectile can be fired
    private Queue<GameObject> projectilePool = new Queue<GameObject>(); // Pool for reusable projectiles
    private string targetTag; // Variable to store the tag of the target object
    private Coroutine firingCoroutine; // Reference to the currently running coroutine

    #region Unity Methods

    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Store the tag of the object that triggered the collider
        targetTag = other.tag;

        // Check if the firing coroutine is not already running
        if (firingCoroutine == null)
        {
            // Start firing projectiles
            firingCoroutine = StartCoroutine(FireProjectiles());
        }
    }

    // This method is called when another collider exits the trigger collider attached to this GameObject
    private void OnTriggerExit(Collider other)
    {
        // Stop firing projectiles when the target exits the trigger area
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine); // Stop the firing coroutine
            firingCoroutine = null; // Reset the coroutine reference
        }
    }
    #endregion

    #region Private Methods

    // Coroutine to handle the instantiation of projectiles at randomly selected spawn points
    private IEnumerator FireProjectiles()
    {
        // Infinite loop to continuously fire projectiles
        while (true)
        {
            // Update the next fire time to the current time plus the fire rate
            nextFireTime = Time.time + fireRate;

            // Check if there are any spawn points available
            if (spawnPoints.Count == 0)
            {
                Debug.LogWarning("No spawn points available to instantiate projectiles.");
                yield break; // Exit the coroutine if there are no spawn points
            }

            // Variable to keep track of the last used spawn point index
            int lastUsedIndex = -1;

            // Iterate through each projectile prefab to instantiate
            for (int i = 0; i < projectilePrefabs.Count; i++)
            {
                // Get a random spawn index while avoiding consecutive usage
                int randomIndex = GetRandomSpawnIndex(ref lastUsedIndex);

                // Get a projectile from the pool or instantiate a new one if the pool is empty
                GameObject pooledProjectile = GetProjectileFromPool(i);

                // Set the projectile's position and rotation based on the selected spawn point
                pooledProjectile.transform.position = spawnPoints[randomIndex].position;
                pooledProjectile.transform.rotation = spawnPoints[randomIndex].rotation;

                // Set the target tag on the projectile (assuming the projectile has a script to handle targeting)
                FlyAtPlayer projectileBehavior = pooledProjectile.GetComponent<FlyAtPlayer>();
                if (projectileBehavior != null)
                {
                    projectileBehavior.SetTargetTag(targetTag); // Set the target tag for the projectile
                }

                // Activate the projectile (assuming it is inactive when pooled)
                pooledProjectile.SetActive(true);

                // Optionally, you can add a delay between firing projectiles
                yield return new WaitForSeconds(fireRate / projectilePrefabs.Count);
            }

            // Wait until the next fire time before continuing the loop
            yield return new WaitUntil(() => Time.time >= nextFireTime);
        }
    }

    // Method to get a random spawn index while avoiding consecutive usage
    private int GetRandomSpawnIndex(ref int lastUsedIndex)
    {
        int randomIndex;

        // Handle cases with very low number of spawn points
        if (spawnPoints.Count == 1)
        {
            randomIndex = 0; // Only one spawn point available
        }
        else if (spawnPoints.Count == 2)
        {
            randomIndex = (lastUsedIndex == 0) ? 1 : 0; // Toggle between 0 and 1
        }
        else
        {
            // For more than two spawn points, ensure that the same spawn point is not used consecutively
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Count); // Generate a random index
            } while (randomIndex == lastUsedIndex); // Repeat until a different index is found
        }

        lastUsedIndex = randomIndex; // Update the last used index to the current one
        return randomIndex; // Return the selected random index
    }

    // Method to get a projectile from the pool or instantiate a new one if the pool is empty
    private GameObject GetProjectileFromPool(int prefabIndex)
    {
        // Check if the projectile pool has any objects
        if (projectilePool.Count > 0)
        {
            // Dequeue a projectile from the pool
            return projectilePool.Dequeue();
        }
        else
        {
            // If the pool is empty, instantiate a new projectile from the specified prefab
            return Instantiate(projectilePrefabs[prefabIndex]);
        }
    }

    // Method to return a projectile back to the pool
    public void ReturnProjectileToPool(GameObject projectile)
    {
        projectile.SetActive(false); // Deactivate the projectile
        projectilePool.Enqueue(projectile); // Add the projectile back to the pool
    }
    #endregion
}