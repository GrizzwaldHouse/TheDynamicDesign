using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawn : MonoBehaviour


{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject SpawnObject;
    private void OnTriggerEnter()
    {
        Instantiate(SpawnObject, SpawnPoint.position, SpawnPoint.rotation);
    }

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

}