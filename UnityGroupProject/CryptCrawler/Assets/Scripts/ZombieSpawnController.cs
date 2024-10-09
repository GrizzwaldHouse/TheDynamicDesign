using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

// This script is responsible for controlling the spawning of zombies in the game.
public class ZombieSpawnController : MonoBehaviour
{
    // The initial number of zombies to spawn per wave.
    [SerializeField] public int initialZombiesPerWave;

    // The current number of zombies to spawn per wave. This value will increase as the game progresses.
    [SerializeField] public int currentZombiesPerWave;

    // The delay between each zombie spawn.
    [SerializeField] public float spawnDelay;

    // The current wave number.
    [SerializeField] public int currentWave;

    // The cooldown time between waves.
    [SerializeField] public float waveCoolDown;

    // The prefab for the zombie game object.
    public GameObject zombiePrefab;

    // The UI text element to display the wave over message.
    public TextMeshProUGUI waveOverUI;

    // The UI text element to display the cooldown counter.
    public TextMeshProUGUI cooldownCounterUI;

    // A flag to indicate whether the game is currently in a cooldown period.
    public bool inCooldown;

    // The cooldown counter timer.
    public float coolDownCounter = 0;

    // A list to store the currently alive zombies.
    public List<enemyAI> currentZombiesAlive;

    // Called when the script is initialized.
    private void Start()
    {
        // Set the initial number of zombies per wave.
        currentZombiesPerWave = initialZombiesPerWave;

        // Start the first wave.
        StartCoroutine(SpawnWave());
    }

    // Called when the current wave is over and the next wave should be started.
    private void StartNextWave()
    {
        // Clear the list of currently alive zombies.
        currentZombiesAlive.Clear();

        // Increment the wave number.
        currentWave++;

        // Start the next wave.
        StartCoroutine(SpawnWave());
    }

    // A coroutine to spawn a wave of zombies.
    private IEnumerator SpawnWave()
    {
        // Loop through the number of zombies to spawn.
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            // Calculate a random spawn offset.
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1));

            // Calculate the spawn position based on the offset and the spawn controller's position.
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantiate a new zombie game object at the spawn position.
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Get the enemy AI script component from the zombie game object.
            enemyAI enemyScript = zombie.GetComponent<enemyAI>();

            // Add the enemy AI script to the list of currently alive zombies.
            currentZombiesAlive.Add(enemyScript);

            // Wait for the spawn delay before spawning the next zombie.
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Called every frame.
    private void Update()
    {
        // Create a list to store zombies that need to be removed.
        List<enemyAI> zombiesToRemove = new List<enemyAI>();

        // Loop through the currently alive zombies.
        foreach (enemyAI zombie in currentZombiesAlive)
        {
            // Check if the zombie is dead.
            if (zombie.isDead)
            {
                // Add the zombie to the list of zombies to remove.
                zombiesToRemove.Add(zombie);
            }
        }

        // Loop through the list of zombies to remove and remove them from the currently alive list.
        foreach (enemyAI zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        // Clear the list of zombies to remove.
        zombiesToRemove.Clear();

        // Check if all zombies are dead and the game is not in a cooldown period.
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            // Start the wave cooldown coroutine.
            StartCoroutine(WaveCooldown());
        }

        // Check if the game is in a cooldown period.
        if (inCooldown)
        {
            // Decrement the cooldown counter.
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            // Reset the cooldown counter.
            coolDownCounter = waveCoolDown;
        }
    }

    // A coroutine to handle the wave cooldown period.
    private IEnumerator WaveCooldown()
    {
        // Set the game to be in a cooldown period.
        inCooldown = true;

        // Wait for the cooldown period to complete.
        yield return new WaitForSeconds(waveCoolDown);

        // Set the game to not be in a cooldown period.
        inCooldown = false;

        // Double the number of zombies per wave.
        currentZombiesPerWave *= 2;

        // Start the next wave.
        StartNextWave();
    }
}
