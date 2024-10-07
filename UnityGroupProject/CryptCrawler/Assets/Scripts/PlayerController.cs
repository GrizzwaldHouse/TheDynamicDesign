using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for controlling the player's movement, shooting, and damage handling.
public class PlayerController : MonoBehaviour, IDamage
{
    // The CharacterController component is used to handle the player's movement and collision detection.
    [SerializeField] CharacterController controller;

    // A LayerMask is used to ignore certain layers when shooting or colliding with objects.
    [SerializeField] LayerMask ignoreMask;

    // The player's height when standing.
    [SerializeField] int playerheight;

    // The player's height when crouching.
    [SerializeField] int crouchHeight;

    // The speed reduction when crouching.
    [SerializeField] int crouchSpeed;

    // The player's movement speed.
    [SerializeField] int speed;

    // The player's health points.
    [SerializeField] int HP;

    // The sprint speed modifier.
    [SerializeField] int sprintmod;

    // The jump speed.
    [SerializeField] int jumpspeed;

    // The gravity value.
    [SerializeField] int gravity;

    // The maximum number of jumps allowed.
    [SerializeField] int jumpmax;

    // The rate at which the player can shoot.
    [SerializeField] float shootRate;

    // The damage dealt by the player's shots.
    [SerializeField] int shootDamage;

    // The maximum distance the player's shots can travel.
    [SerializeField] int shootDist;

    // The direction of the player's movement.
    Vector3 moveDir;

    // The player's velocity.
    Vector3 playerVel;

    // The number of jumps the player has made.
    int jumpcount;

    // Whether the player is currently sprinting.
    bool isSprinting;

    // Whether the player is currently shooting.
    bool isShooting;

    // Start is called before the first frame update.
    void Start()
    {
        // Initialize any necessary variables or components here.
    }

    // Update is called once per frame.
    void Update()
    {
        // Draw a raycast from the camera to visualize the shooting distance.
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

        // Call the movement function to handle player movement.
        movement();

        // Call the sprint function to handle sprinting.
        sprint();

        // Call the crouch function to handle crouching.
        crouch();
    }

    // This function handles the player's movement.
    void movement()
    {
        // Get the horizontal and vertical input from the player.
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        // Move the player based on the input and speed.
        controller.Move(moveDir * speed * Time.deltaTime);

        // If the player is grounded, reset the jump count and velocity.
        if (controller.isGrounded)
        {
            jumpcount = 0;
            playerVel = Vector3.zero;
        }

        // If the player presses the jump button and has not exceeded the maximum jump count, apply a jump force.
        if (Input.GetButtonDown("Jump") && jumpcount < jumpmax)
        {
            jumpcount++;
            playerVel.y = jumpspeed;
        }

        // Apply gravity to the player's velocity.
        playerVel.y -= gravity * Time.deltaTime;

        // Move the player based on the velocity.
        controller.Move(playerVel * Time.deltaTime);

        // If the player presses the fire button and is not currently shooting, start the shooting coroutine.
        if (Input.GetButton("Fire1") && !gamemanager.instance.isPaused && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    // This function handles the player's sprinting.
    void sprint()
    {
        // If the player presses the sprint button, increase the speed.
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintmod;
        }
        // If the player releases the sprint button, decrease the speed.
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintmod;
        }
    }

    // This function handles the player's crouching.
    void crouch()
    {
        // If the player presses the crouch button, decrease the height and speed.
        if (Input.GetButtonDown("Crouch"))
        {
            controller.height = crouchHeight;
            speed -= crouchSpeed;
        }
        // If the player releases the crouch button, increase the height and speed.
        else if (Input.GetButtonUp("Crouch"))
        {
            controller.height = playerheight;
            speed += crouchSpeed;
        }
    }

    // This coroutine handles the player's shooting.
    IEnumerator shoot()
    {
        // Set the isShooting flag to true to prevent multiple shots from being fired at once.
        isShooting = true;

        // Create a RaycastHit to store the result of the raycast.
        RaycastHit hit;

        // Cast a ray from the camera's position in the direction of the camera's forward vector, 
        // with a maximum distance of shootDist, and ignoring any layers specified in the ignoreMask.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            // Get the IDamage component from the hit object's collider.
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            // If the hit object has an IDamage component, call its takeDamage method with the shootDamage amount.
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        // Wait for the shootRate amount of time before allowing the player to shoot again.
        yield return new WaitForSeconds(shootRate);

        // Set the isShooting flag to false to allow the player to shoot again.
        isShooting = false;
    }

    // This method is called when the player takes damage.
    public void takeDamage(int amount)
    {
        // Subtract the damage amount from the player's health points.
        HP -= amount;

        // If the player's health points are less than or equal to 0, call the youLose method on the gamemanager instance.
        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }
}