using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    [SerializeField] int initalZombiePerWaze;
    [SerializeField] int currentZombiePerWave;

    [SerializeField] int spawnDelay;
    [SerializeField] int currentWave;
    [SerializeField] float waveCooldown;
    public bool inCoolDown;
    public float coolDownCounter = 0;
    public List<EnemyAI> currentZombieAlive;
    public GameObject zombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        currentZombiePerWave = initalZombiePerWaze;
    }
}

    // Update is called once per frame
//    private void StartNextWave()
//    {
//        currentZombieAlive.Clear();
//        currentWave++;
//        StartCoroutine(SpawnWave());

//    }

//    private IEnumerator SpawnWave()
//    {
//        for(int i = 0; i < currentZombiePerWave; i++) {
//            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
//            Vector3 sspawnPosition = transform.position + spawnOffset;
//            var zombie = Instantiate(zombiePrefab, sspawnPosition, Quaternion.identity);
//            enemyAI emenyScipt = zombie.GetComponent<enemyAI>();
//            currentZombieAlive.Add(emenyScipt);
//            yield return new WaitForSeconds(spawnDelay);
//        }
//}
    //private void Update()
    //{
    //    List<enemyAI>zombiesToRemove = new List<enemyAI>();
    //    foreach (var zombie in currentZombieAlive)
    //    {
    //        if (zombie.isDead)
    //        {
    //            currentZombieAlive.Remove(zombie); 
    //        }
    //    }

    //}