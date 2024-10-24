using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.UI;
// Class representing the AI behavior of the Lava Golem
public class BabLava : MonoBehaviour, IDamage
{
    // Serialized fields to expose variables in the Unity Inspector
    [SerializeField] NavMeshAgent agent; // Reference to the NavMeshAgent for pathfinding
    [SerializeField] Renderer model; // Reference to the Renderer for visual representation
    [SerializeField] Animator anim; // Reference to the Animator for controlling animations
    [SerializeField] int HP; // Current health points of the Lava Golem
    [SerializeField] int rotateSpeed; // Speed at which the Golem rotates to face the player
    [SerializeField] int meleeDamage; // Damage dealt to the player on melee attack
    [SerializeField] float meleeRange; // Range within which the Golem can attack the player
    [SerializeField] Image enemyHPbar; // UI element to display the enemy's health
    [SerializeField] int ExpWorth;

    // Private variables to manage the state of the Golem
    bool isDead; // Flag to check if the Golem is dead
    int HPorig; // Original health points for calculating health percentage
    bool isHit;
    bool isAttacking;// Flag to prevent multiple hits during an attack

    Vector3 playerDir; // Direction vector pointing towards the player

    Color colorOg; // Original color of the Golem for flashing effect

    // Start is called before the first frame update
    void Start()
    {
        // Store the original color of the model for later use
        colorOg = model.material.color;

        // Initialize the original health points
        HPorig = HP;

        // Set the initial destination of the Golem to the player's position
        agent.SetDestination(gamemanager.instance.player.transform.position);
        UpdateEnemyUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Golem is not dead
        if (!isDead)
        {
            // Continuously set the destination to the player's position
            agent.SetDestination(gamemanager.instance.player.transform.position);

            // Update the animation speed based on the agent's velocity
            anim.SetFloat("speed", agent.velocity.normalized.magnitude);

            // Check if the player is within melee range
            if (Vector3.Distance(transform.position, gamemanager.instance.player.transform.position) <= meleeRange)
            {
                // Face the player to prepare for the melee attack
                faceTarget();

                // If the Golem is not currently attacking, start the melee attack coroutine
                if (isHit == false)
                {
                    StartCoroutine(MeleeAttack());
                }
            }
        }
    }

    // Coroutine to handle the melee attack
    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        // Set the hit flag to true to prevent further attacks during this animation
        isAttacking = true;
        // Trigger the attack animation
        anim.SetTrigger("attack");

        // Wait for the duration of the attack animation before proceeding
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);


        // Reset the hit flag to allow future attacks
        isAttacking = false;
    }

    // Method to rotate the Golem to face the player
    void faceTarget()
    {
        // Calculate the direction vector from the Golem to the player
        playerDir = gamemanager.instance.player.transform.position - transform.position;

        // Create a rotation that looks in the direction of the player
        Quaternion rotate = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));

        // Smoothly rotate the Golem towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * rotateSpeed);
    }

    // Method to handle damage taken by the Golem
    public void takeDamage(int amount)
    {
        isHit = true;
        HP -= amount;
        UpdateEnemyUI();

        anim.SetTrigger("hit");
        agent.SetDestination(gamemanager.instance.player.transform.position);

        StartCoroutine(flashColor());

        isHit = false;
        if (HP <= 0)
        {
            gamemanager.instance.UpdateGameGoal(-1);
            gamemanager.instance.accessPlayer.gainExperience(ExpWorth);
            Destroy(gameObject);
        }
    }

    // Coroutine to flash the Golem's color to indicate damage
    IEnumerator flashColor()
    {
        // Set the Golem's color to red to indicate damage
        model.material.color = Color.red;

        // Wait for a short duration before resetting the color
        yield return new WaitForSeconds(0.1f);

        // Reset the Golem's color to its original value
        model.material.color = colorOg;
    }

    // Method to update the enemy UI to reflect the current health
    public void UpdateEnemyUI()
    {
        // Calculate the health percentage based on the current and original health
        enemyHPbar.fillAmount = (float)HP / HPorig;
    }

    // Method to handle health gain (currently empty, but can be implemented as needed)
    public void gainHealth(int amount)
    {

    }
}