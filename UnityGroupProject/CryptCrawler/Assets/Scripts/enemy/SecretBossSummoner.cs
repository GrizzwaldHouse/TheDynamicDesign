using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Class representing the boss that can summon objects and perform various attacks
public class SecretBossSummoner : MonoBehaviour, IDamage
{
    // Enum to define different attack types the boss can perform
    private enum AttackType
    {
        RegularAttack, // Standard attack
        AoeAttack, // Area of Effect attack
        Summoning // Summoning additional objects
    }

    // Serialized fields to expose variables in the Unity Inspector
    [SerializeField] NavMeshAgent agent; // Reference to the NavMeshAgent for movement
    [SerializeField] Renderer model; // Reference to the boss's model for visual representation
    [SerializeField] Animator anim; // Reference to the Animator for controlling animations
    [SerializeField] Transform shootPos; // Position from which the boss will shoot projectiles
    [SerializeField] Transform headPos; // Position of the boss's head for aiming
    [SerializeField] int viewAngle; // Angle within which the boss can see the player

    [SerializeField] int roamDist; // Distance the boss can roam
    [SerializeField] int roamPauseTime; // Time the boss pauses while roaming
    [SerializeField] int ExpWorth; // Experience points awarded for defeating the boss

    [SerializeField] public int HP; // Current health points of the boss
    [SerializeField] int rotateSpeed; // Speed at which the boss rotates to face the player

    [SerializeField] GameObject bullet; // Prefab of the bullet to be instantiated
    [SerializeField] float shootRate; // Rate at which the boss can shoot
    [SerializeField] int shootAngle; // Angle within which the boss can shoot
    [SerializeField] Image enemyHPbar; // UI element to display the boss's health
    [SerializeField] int aoeRadius; // Radius for the Area of Effect attack
    [SerializeField] int aoeDamage; // Damage dealt by the Area of Effect attack
    [SerializeField] GameObject aoeEffect; // Visual effect for the AoE attack
    [SerializeField] private GameObject spawnObject; // The object to spawn
    [SerializeField] private List<Transform> spawnPoints; // List of spawn points for summoning objects
    [SerializeField] private float spawnInterval; // Time interval between spawns
    [SerializeField] private int maxDestroyedCount; // Maximum number of spawned objects before disabling the spawner

    // List to keep track of all spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Hitboxes for the boss
    [SerializeField] private List<Collider> hitboxes; // List of hitboxes for collision detection
    [SerializeField] private float flashDuration; // Duration for which hitboxes flash
    [SerializeField] private float flashInterval; // Time interval for flashing hitboxes

    // State variables to manage boss behavior
    private Collider mainCollider; // Reference to the main collider of the boss
    bool isShooting; // Flag to check if the boss is currently shooting
    bool SecondPhase; // Flag to indicate if the boss is in the second phase
    bool playerInRange; // Flag to check if the player is within range
    bool isRoaming;
    public bool isDead; // Flag to check if the boss is dead
    int HPorig; // Original health points of the boss
    bool isHit; // Flag to check if the boss is currently hit
    private AttackType chosenAttackType; // Variable to store the chosen attack type
    float angleToplayer; // Angle to the player for aiming
    float stoppingDistOrig; // Original stopping distance for the NavMeshAgent
    private int currentDestroyedCount; // Counter for the number of destroyed spawned objects
    Vector3 playerDir; // Direction vector to the player

    Vector3 startingPos; // Starting position of the boss
    Color colorOg; // Original color of the boss's model

    Coroutine someCo; // Coroutine reference for managing coroutines
    Coroutine spawnCoroutine; // Coroutine reference for spawning objects
    Coroutine flashCoroutine; // Coroutine reference for flashing hitboxes

    // Start is called before the first frame update
    void Start()
    {
        colorOg = model.material.color; // Store the original color of the boss gamemanager.instance.UpdateGameGoal(1); // Update the game goal when the boss is instantiated
        UpdateEnemyUI(); // Update the UI to reflect the boss's health
        HPorig = HP; // Store the original health points for reference
        startingPos = transform.position; // Record the starting position of the boss
        stoppingDistOrig = agent.stoppingDistance; // Store the original stopping distance for the agent
        isDead = false; // Initialize the dead state to false
        chosenAttackType = AttackType.RegularAttack; // Set the initial attack type to RegularAttack
        mainCollider = GetComponent<Collider>(); // Get the main collider component
        if (mainCollider != null)
        {
            mainCollider.enabled = false; // Disable the collider initially
        }
        flashCoroutine = StartCoroutine(FlashHitBoxes()); // Start the coroutine to flash hitboxes
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in range and can be seen
        if (playerInRange && canSeePlayer())
        {
            ChooseAttackPattern(); // Choose an attack pattern based on the current state
        }
    }

    // Method to check if the boss can see the player
    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - headPos.position; // Calculate direction to the player
        angleToplayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward); // Calculate angle to the player
        Debug.DrawRay(headPos.position, playerDir); // Draw a debug ray for visualization

        RaycastHit hit; // Variable to store raycast hit information
        if (Physics.Raycast(headPos.position, playerDir, out hit)) // Perform a raycast to check for obstacles
        {
            if (hit.collider.CompareTag("Player") && angleToplayer <= viewAngle) // Check if the hit object is the player and within view angle
            {
                agent.SetDestination(gamemanager.instance.player.transform.position); // Move the boss towards the player
                if (agent.remainingDistance <= agent.stoppingDistance) // Check if the boss is close enough to the player
                {
                    faceTarget(); // Face the player
                }

                // Check if the boss is not currently shooting and is within shooting angle
                if (!isShooting && angleToplayer < shootAngle)
                {
                    // Choose attack type based on the phase of the boss
                    if (SecondPhase)
                    {
                        chosenAttackType = (AttackType)Random.Range(0, 3); // Randomly choose an attack type in the second phase
                    }
                    else
                    {
                        chosenAttackType = AttackType.RegularAttack; // Always use regular attack in the first phase
                    }

                    // Execute the chosen attack type
                    switch (chosenAttackType)
                    {
                        case AttackType.RegularAttack:
                            StartCoroutine(Shoot()); // Start shooting coroutine
                            break;
                        case AttackType.AoeAttack:
                            AoeAttack(); // Perform AoE attack
                            break;
                        case AttackType.Summoning:
                            SpawnObject(); // Summon an object
                            break;
                    }
                }
                agent.stoppingDistance = stoppingDistOrig; // Reset the stopping distance
                return true; // Player is visible
            }
        }

        agent.stoppingDistance = 0; // Reset stopping distance if player is not visible
        return false; // Player is not visible
    }

    // Method to make the boss face the player
    void faceTarget()
    {
        Quaternion rotate = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z)); // Calculate rotation towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * rotateSpeed); // Smoothly rotate towards the player
    }

    // Coroutine to flash hitboxes for visual feedback
    private IEnumerator FlashHitBoxes()
    {
        while (true) // Infinite loop to continuously flash hitboxes
        {
            foreach (var hitbox in hitboxes) // Iterate through each hitbox
            {
                hitbox.enabled = true; // Enable the hitbox
            }
            yield return new WaitForSeconds(flashDuration); // Wait for the flash duration
            foreach (var hitbox in hitboxes) // Iterate through each hitbox again
            {
                hitbox.enabled = false; // Disable the hitbox
            }
            yield return new WaitForSeconds(flashInterval); // Wait for the flash interval
        }
    }

    // Trigger event when another collider enters the boss's trigger collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            playerInRange = true; // Set playerInRange to true when the player enters the trigger
        }
    }

    // Trigger event when another collider exits the boss's trigger collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            playerInRange = false; // Set playerInRange to false when the player exits the trigger
            agent.stoppingDistance = 0; // Reset stopping distance when the player is out of range
        }
    }

    // Method to choose an attack pattern based on the boss's health
    private void ChooseAttackPattern()
    {
        if (HP > HPorig / 2) // If health is above half
        {
            if (Random.value < 0.5f) // Randomly decide between two attack types
            {
                StartCoroutine(Shoot()); // Start shooting coroutine
            }
            else
            {
                AoeAttack(); // Perform AoE attack
            }
        }
        else // If health is below half
        {
            if (Random.value < 0.3f) // Randomly decide to summon an object
            {
                SpawnObject(); // Call to spawn an object
            }
            else
            {
                StartCoroutine(SpecialAttack()); // Start special attack coroutine
            }
        }
    }

    // Coroutine to handle shooting behavior
    IEnumerator Shoot()
    {
        if (!isHit) // Check if the boss is not currently hit
        {
            isShooting = true; // Set shooting flag to true
            anim.SetTrigger("attack"); // Trigger the attack animation
            yield return new WaitForSeconds(shootRate); // Wait for the shoot rate duration
            Instantiate(bullet, shootPos.position, transform.rotation); // Instantiate the bullet at the shoot position

            isShooting = false; // Reset shooting flag
        }
    }

    // Method to handle taking damage
    public void takeDamage(int amount)
    {
        isHit = true; // Set hit flag to true
        HP -= amount; // Reduce health by the damage amount
        UpdateEnemyUI(); // Update the UI to reflect the new health

        if (someCo != null) // If a coroutine is running
        {
            StopCoroutine(someCo); // Stop the roaming coroutine
            isRoaming = false; // Set roaming flag to false
        }
        anim.SetTrigger("hit"); // Trigger the hit animation
        agent.SetDestination(gamemanager.instance.player.transform.position); // Move towards the player

        StartCoroutine(flashColor()); // Start the flash color coroutine for visual feedback

        if (HP <= 0) // Check if health has dropped to zero or below
        {
            gamemanager.instance.UpdateGameGoal(-1); // Update the game goal to reflect the boss's defeat
            isDead = true; // Set dead flag to true
            gamemanager.instance.accessPlayer.gainExperience(ExpWorth); // Award experience to the player
            Destroy(gameObject); // Destroy the boss game object
        }
        isHit = false; // Reset hit flag
    }

    // Coroutine for special attack behavior
    private IEnumerator SpecialAttack()
    {
        anim.SetTrigger("specialAttack"); // Trigger the special attack animation
        yield return new WaitForSeconds(1f); // Wait for the telegraph animation
        SpawnObject(); // Call to spawn an object during the special attack
    }

    // Coroutine to flash the boss's color when hit
    IEnumerator flashColor()
    {
        model.material.color = Color.red; // Change the model's color to red
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        model.material.color = colorOg; // Reset the model's color to the original
    }

    // Method to update the enemy health UI
    public void UpdateEnemyUI()
    {
        enemyHPbar.fillAmount = (float)HP / HPorig; // Update the health bar fill amount based on current health
    }

    // Method to gain health (currently empty for future implementation)
    public void gainHealth(int amount)
    {
        // Future implementation for gaining health
    }

    // Method to check if the boss should transition to the second phase
    void CheckSecondPhase()
    {
        if (!SecondPhase && HP <= HPorig / 2) // If not in second phase and health is below half
        {
            SecondPhase = true; // Transition to the second phase
            Debug.Log("Boss has entered the second phase!"); // Log the phase transition
            agent.speed *= 2; // Increase the boss's speed in the second phase
            spawnCoroutine = StartCoroutine(SpawnObjectsAtIntervals()); // Start the coroutine to spawn objects at intervals during the second phase
        }
    }

    // Method to perform an Area of Effect attack
    void AoeAttack()
    {
        Debug.Log("Performing AoE attack!"); // Log the AoE attack

        // Instantiate the AoE particle effect at the boss's position
        if (aoeEffect != null)
        {
            Instantiate(aoeEffect, transform.position, Quaternion.identity); // Create the AoE effect
        }

        // Wait for a short duration before applying damage
        new WaitForSeconds(1f);

        // Find all colliders within the AoE radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);

        foreach (var hitCollider in hitColliders) // Iterate through all colliders in the area
        {
            // Check if the collider belongs to a player
            if (hitCollider.CompareTag("Player"))
            {
                // Apply damage to the player
                hitCollider.GetComponent<PlayerController>().takeDamage(aoeDamage); // Call the takeDamage method on the player
            }
        }
    }

    // Method to spawn an object at a random spawn point
    void SpawnObject()
    {
        if (currentDestroyedCount >= maxDestroyedCount) // Check if the maximum destroyed count is reached
        {
            return; // Exit the method if the limit is reached
        }
        if (spawnPoints == null || spawnPoints.Count == 0 || spawnObject == null) // Check if spawn points or spawn object is not set
        {
            Debug.LogError("Spawn points or spawn object is not set."); // Log an error message
            return; // Exit the method
        }
        // Randomly select a spawn point from the list
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instantiate the spawn object at the selected spawn point's position and rotation
        GameObject spawnedObject = Instantiate(spawnObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        // Add the spawned object to the list for tracking
        spawnedObjects.Add(spawnedObject);
        currentDestroyedCount++; // Increment the destroyed count
    }

    // Method to handle when a spawned object is destroyed
    public void ObjectDestroyed(GameObject destroyedObject)
    {
        if (spawnedObjects.Contains(destroyedObject)) // Check if the destroyed object is in the list
        {
            spawnedObjects.Remove(destroyedObject); // Remove the destroyed object from the list
            currentDestroyedCount++; // Increment the destroyed count

            // Disable the spawner if the max destroyed count is reached
            if (currentDestroyedCount >= maxDestroyedCount)
            {
                DestroyAllSpawnedObjects(); // Call to destroy all spawned objects
            }
        }
    }

    // Method to destroy all spawned objects
    private void DestroyAllSpawnedObjects()
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

    // Coroutine to spawn objects at regular intervals during the second phase
    IEnumerator SpawnObjectsAtIntervals()
    {
        while (SecondPhase) // Continue spawning while in the second phase
        {
            SpawnObject(); // Call to spawn an object
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified spawn interval
        }
    }

    // Method to visualize the AoE radius in the editor
    void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Draw a wire sphere at the boss's position with the AoE radius
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}