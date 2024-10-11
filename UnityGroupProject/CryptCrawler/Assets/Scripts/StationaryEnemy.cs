using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : MonoBehaviour, IDamage
{


    // Renderer is a Unity component that handles the visual representation of the enemy
    [SerializeField] Renderer model;



    // HP (Health Points) is an integer that represents the enemy's health
    [SerializeField] int HP;

    // rotateSpeed is an integer that determines how fast the enemy rotates to face the player
    [SerializeField] int rotateSpeed;

    // bullet is a GameObject that represents the enemy's projectile
    [SerializeField] GameObject bullet;

    // shootRate is a float that determines how often the enemy shoots
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    // playerInRange is a boolean that indicates whether the player is within range of the enemy
    bool playerInRange;

    // isShooting is a boolean that indicates whether the enemy is currently shooting
    bool isShooting;

    // colorOrig is a Color that stores the original color of the enemy's model
    Color colorOrig;

    // playerDir is a Vector3 that stores the direction from the enemy to the player
    Vector3 playerDir;

    // Start is a Unity method that is called before the first frame update
    void Start()
    {
        // Store the original color of the enemy's model
        colorOrig = model.material.color;

        // Update the game goal by 1 using the GameManager instance
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is a Unity method that is called once per frame
    void Update()
    {
        // If the player is within range of the enemy
        if (playerInRange)
        {
            // Calculate the direction from the enemy to the player
            playerDir = gamemanager.instance.player.transform.position - transform.position;


            // If the enemy is not currently shooting
            if (!isShooting)
            {
                // Start a coroutine to shoot at the player
                StartCoroutine(shoot());
            }
        }
    }

    // faceTarget is a method that makes the enemy face the player


    // OnTriggerEnter is a Unity method that is called when another collider enters the enemy's trigger collider
    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Set playerInRange to true
            playerInRange = true;
        }
    }

    // OnTriggerExit is a Unity method that is called when another collider exits the enemy's trigger collider
    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Set playerInRange to false
            playerInRange = false;
        }
    }

    // shoot is a coroutine that makes the enemy shoot at the player
    IEnumerator shoot()
    {
        // Set isShooting to true
        isShooting = true;

        // Instantiate a bullet at the shootPos position with the enemy's rotation
        Instantiate(bullet, shootPos.position, transform.rotation);

        // Wait for the shootRate time before allowing the enemy to shoot again
        yield return new WaitForSeconds(shootRate);

        // Set isShooting to false
        isShooting = false;
    }

    // takeDamage is a method that is called when the enemy takes damage
    public void takeDamage(int amount)
    {
        // Subtract the damage amount from the enemy's HP
        HP -= amount;

        // Start a coroutine to flash the enemy's color
        StartCoroutine(flashColor());

        // If the enemy's HP is less than or equal to 0
        if (HP <= 0)
        {
            // Update the game goal by -1 using the GameManager instance
            gamemanager.instance.updateGameGoal(-1);

            // Destroy the enemy game object
            Destroy(gameObject);
        }
    }

    // flashColor is a coroutine that makes the enemy's color flash red
    IEnumerator flashColor()
    {
        // Set the enemy's material color to red
        model.material.color = Color.red;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Set the enemy's material color back to its original color
        model.material.color = colorOrig;
    }
}
