using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script is responsible for controlling the enemy AI and handling damage.
public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    // This variable is exposed to the Unity Inspector, allowing designers to assign the renderer component of the enemy model.
    [SerializeField] Renderer model;

    // This variable is exposed to the Unity Inspector, allowing designers to set the initial health points (HP) of the enemy.
    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] int viewAngle;
    [SerializeField] GameObject bullet;  // bullet is a GameObject that represents the enemy's projectile
    [SerializeField] float shootRate;  // shootRate is a float that determines how often the enemy shoots
    // This variable stores the original color of the enemy model.
    Color colorOrig;
    bool isShooting;

    public bool isDead;
    float angleToPlayer;
    bool playerInRange; // playerInRange is a boolean that indicates whether the player is within range of the enemy
    Vector3 playerDir;   // playerDir is a Vector3 that stores the direction from the enemy to the player
    // This method is called once at the start of the game.
    void Start()
    {
        // Store the original color of the enemy model.
        colorOrig = model.material.color;

        // Notify the game manager that a new enemy has spawned, incrementing the enemy count.
        gamemanager.instance.updateGameGoal(1);
    }

    // This method is called every frame, but is currently empty.
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            {

            }
        }
        // TO DO: Add AI logic here, such as movement, attack, or patrol behaviors.
    }

    // This method is called when the enemy takes damage.
    public void takeDamage(int amount)
    {
        // Subtract the damage amount from the enemy's HP.
        HP -= amount;

        // Start a coroutine to flash the enemy's color to indicate damage.
        StartCoroutine(flashColor());

        // If the enemy's HP reaches 0 or less, destroy the enemy game object and notify the game manager.
        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    bool canSeePlayer()
    {

        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        agent.SetDestination(gamemanager.instance.transform.position);
        angleToPlayer = Vector3.Angle(transform.forward, playerDir);
        Debug.DrawRay(headPos.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gamemanager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                   
                }
            }
            return true;

        }
        return false;
    }
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }
        void faceTarget()
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        }
        IEnumerator shoot()
        {
            isShooting = true;
            yield return new WaitForSeconds(shootRate);
            GameObject bulletInstance = Instantiate(bullet, shootPos.position, shootPos.rotation);
            bulletInstance.GetComponent<Rigidbody>().velocity = shootPos.forward * 10;
            isShooting = false;
        }
    

    // This coroutine flashes the enemy's color to indicate damage.
    IEnumerator flashColor()
    {
        // Set the enemy model's color to red to indicate damage.
        model.material.color = Color.red;

        // Wait for 0.1 seconds to create a brief flash effect.
        yield return new WaitForSeconds(0.1f);

        // Restore the enemy model's original color.
        model.material.color = colorOrig;
    }
}
