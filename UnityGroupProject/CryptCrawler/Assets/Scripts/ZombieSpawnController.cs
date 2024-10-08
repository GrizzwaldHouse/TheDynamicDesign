using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
public class ZombieSpawnController : MonoBehaviour
{
    [SerializeField] public int intialZombiesPerWave;
    [SerializeField] public int currentZombiesPerWave;
    [SerializeField] public float spawnDelay;
    [SerializeField] public int currentWave;
    [SerializeField] public float waveCoolDown;
    public GameObject zombiePrefab;
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public bool inCooldown;
    public float coolDownCounter = 0;
    public List<enemyAI> currentZombiesAlive;
    private void Start()
    {
        currentZombiesPerWave = intialZombiesPerWave;
        StartCoroutine(SpawnWave());

    }
    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1));
            Vector3 spawnPosition = transform.position + spawnOffset;
            // var zombie = Instantiate(Resources.Load(zombiePrefab, spawnPosition, Quaternion.identity);
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            enemyAI enemyScipt = zombie.GetComponent<enemyAI>();
            currentZombiesAlive.Add(enemyScipt);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private void Update()
    {
        List<enemyAI> zombiesToRemove = new List<enemyAI>();
        foreach (enemyAI zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);

            }
        }
        foreach (enemyAI zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }
        if (inCooldown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCoolDown;
        }

    }
    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(waveCoolDown);
        inCooldown = false;
        currentZombiesPerWave *= 2;
       StartNextWave(); 
    }
}

