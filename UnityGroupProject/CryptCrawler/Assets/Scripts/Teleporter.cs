using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update

    //Teleport player to a different area when triggered
    public Transform player, destination;
    // Declare a public GameObject variable: playerg
    // This will be set in the Unity editor to specify the player's game object
    public GameObject playerChar;
    [Header("Health Reset Options")]
    [SerializeField] private bool resetEnemyHealthONTeleport = true;

    // Declare a public GameObject variable: playerg
    // This will be set in the Unity editor to specify the player's game object
    private void OnTriggerEnter(Collider other)
    {

        // Check if the collider that entered the trigger has a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Set the player game object to be inactive (i.e., hide it)
            playerChar.SetActive(false);
            // Set the player's position to the destination's position (i.e., teleport the player)
            player.position = destination.position;
            // Set the player game object to be active again (i.e., show it at the new position)
            playerChar.SetActive(true);
            if (resetEnemyHealthONTeleport)
            {
                IEnemy[] enemies = FindObjectsOfType<MonoBehaviour>().OfType<IEnemy>().ToArray();
                foreach (IEnemy enemy in enemies)
                {
                    enemy.ResetHealth(); 
                }
            }
        }
    }
}
