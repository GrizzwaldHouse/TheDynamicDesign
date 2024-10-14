using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] int HP;
    [SerializeField] int mana;
    [SerializeField] int experience;
    [SerializeField] int level;
    [SerializeField] int experienceToNextLevel;
    [SerializeField] int speed;
    [SerializeField] int crouchspeed;
    [SerializeField] float maxslidetime;
    [SerializeField] float slideforce;
    [SerializeField] float slidetimer;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] GameObject spell;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
   
    int HPorig;
    int ManaOrig;
    int origSpeed;
    
    bool isSprinting;
    bool isSliding;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        ManaOrig = mana;
        origSpeed = speed;
        UpdatePlayerUI();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        movement();
        Sprint();
        crouch();
       
        
    }

    void movement()
    {
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += moveDir * speed * Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButton("Shoot") && !gamemanager.instance.isPaused && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    void crouch()
    {
        
            if (Input.GetButtonDown("Crouch"))
            {
            speed = crouchspeed;
            controller.height = 1;
            
            
            }
        if (Input.GetButtonUp("Crouch") && !isSliding)
        {
            speed = origSpeed;
            controller.height = 2;
           
            
        }
    }

    
    

    IEnumerator Shoot()
    {
        if (mana >= spell.GetComponent<Mana>().Manacost)
        {
            isShooting = true;

            // Instantiate the spell
            GameObject spellInstance = Instantiate(spell, shootPos.position, transform.rotation);

            // Get the spell's script
            Mana spellScript = spellInstance.GetComponent<Mana>();

            // Deduct the mana cost from the player's mana
            mana -= spellScript.Manacost;

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            // Handle the case where the player doesn't have enough mana
            Debug.Log("Not enough mana to cast the spell!");
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdatePlayerUI();
        StartCoroutine(DamageFlash());

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    public void gainHealth(int amount)
    {
        HP = Mathf.Min(HP + amount, HPorig);
        UpdatePlayerUI();
    }
    public void gainExperience(int amount)
    {
        if(experience>= experienceToNextLevel)
        {
            levelUp();
        }
    }
    public int GetLevel() { return level; }
    public int GetHealth() { return HP; }
    void levelUp()
    {
        level++;
        experience = calculateExperienceToNextLevel();
        experienceToNextLevel = calculateExperienceToNextLevel();
        HP = HPorig; //reset health to max
        UpdatePlayerUI();
        Debug.Log("Level Up! You are now level" + level);

    }
    private int calculateExperienceToNextLevel()
    {
        return (int)Mathf.Pow(level, 2) * 100;
    }
   

    IEnumerator DamageFlash()
    {
        gamemanager.instance.PlayerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.PlayerDamageScreen.SetActive(false);
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadSystem()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        level = data.level;
        HP = data.health;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
    public void UpdatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPorig;
        
    }
}