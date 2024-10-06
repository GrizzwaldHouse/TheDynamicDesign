using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] int playerheight;
    [SerializeField] int crouchHeight;
    [SerializeField] int crouchSpeed;
    [SerializeField] int speed;
   
    [SerializeField] int HP;
    [SerializeField] int sprintmod;
    [SerializeField] int jumpspeed;
    [SerializeField] int gravity;
    [SerializeField] int jumpmax;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpcount;
    bool isSprinting;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);
        movement();
        sprint();
        crouch();
        

    }
    void movement()
    {

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (controller.isGrounded)
        {
            jumpcount = 0;
            playerVel = Vector3.zero;
        }
        if (Input.GetButtonDown("Jump") && jumpcount < jumpmax)
        {
            jumpcount++;
            playerVel.y = jumpspeed;

        }
        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
        if (Input.GetButton("Fire1") && !gamemanager.instance.isPaused && !isShooting)
        {
            StartCoroutine(shoot());
        }
        

    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintmod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintmod;
        }
    }
    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            controller.height = crouchHeight;
            speed -= crouchSpeed;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            controller.height = playerheight;
            speed += crouchSpeed;

        }
    }
    
    IEnumerator shoot()
    {
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

   

    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
           gamemanager.instance.youLose();
        }
    }
}
