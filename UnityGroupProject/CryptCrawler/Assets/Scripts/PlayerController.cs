using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] public int HP;
    [SerializeField] public int mana;
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
    [SerializeField] List<Wands> Wandlist = new List<Wands>();
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] float shieldDuration;
    [SerializeField] LayerMask enemyLayer; // Layer for enemies
    [SerializeField] GameObject spell;
    [SerializeField] public Transform shootPos;
    [SerializeField] public Transform headPos;
    [SerializeField] float shootRate;
    [SerializeField] GameObject wandModel;
    [SerializeField] float skillCooldownDuration;
    [SerializeField] int healedAmount;
    [SerializeField] int manaRestored;
    [SerializeField] int healthBoosted;
    [SerializeField] int manaBoosted;
    private float skillCooldown; // Tracks the cooldown time
    private GameObject currentShield;
    public int selectItemPos;
    public List<ItemData> itemList = new List<ItemData>();
    [SerializeField] GameObject inventoryModel;
    public static PlayerController instance;
    public string areaTransitionName;
    public string currentQuestName; // Current quest name



    Vector3 moveDir;
    Vector3 playerVel;

    public TextMeshProUGUI currentQuest;

    int jumpCount;
    int selectwandPos;
    int selectedSkillIndex;
    public int HPorig;
    int XPorig;
    public int ManaOrig;
    int origSpeed;
    float damageAmount;
    public TextMeshProUGUI playerQuest;
    
    bool isSprinting;
    bool isSliding;
    bool isShooting;
    bool SkillUsed;
    public bool hasQuest;
   

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        HPorig = HP;
        ManaOrig = mana;
        XPorig = experience;
        //SetMaxHealth(HPorig);
        //SetMaxMana(ManaOrig);
        origSpeed = speed;
        UpdatePlayerUI();
        UpdatePlayerMana();
        
       
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!gamemanager.instance.isPaused)
        {
            movement();
            selectWand();
            // Check for ability activation
            if (Input.GetButtonDown("Fire2") && Wandlist.Count > 0)
            {
                ActivateWandAbility();
            }
            if (Input.GetButtonDown("Shield")) // Change "Fire3" to whatever button you want
            {
                ActivateShield();
            }

            if (skillCooldown > 0)
        {
            skillCooldown -= Time.deltaTime; // Decrease cooldown timer
        }
        }
    
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


    void ActivateWandAbility()
    {
        if (Wandlist.Count > 0)
        {
            Wands currentWand = Wandlist[selectwandPos];

                if (currentWand.skill != null && skillCooldown <= 0) // Check cooldown
                {
                    currentWand.skill.Activate(this);
                    skillCooldown = skillCooldownDuration; // Reset cooldown
                }
            
        }
    }
     
        void ActivateShield()
        {
            if (currentShield == null) // Check if a shield is already active
            {
                // Instantiate the shield at the player's position
                currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                currentShield.transform.SetParent(transform); // Optionally set parent to the player

                // Destroy the shield after the specified duration
                Destroy(currentShield, shieldDuration);
            }
        }


    IEnumerator Shoot()
    {
        if (mana >= spell.GetComponent<Mana>().Manacost)
        {
            isShooting = true;

            // Instantiate the spell
            GameObject spellInstance = Instantiate(spell, shootPos.position, shootPos.rotation);
            // Raycast to check if the spell hits the enemy
            RaycastHit hit;
           
            if (Physics.Raycast(shootPos.position, transform.forward, out hit, 100f))
            {
                // Check if the hit object is an enemy
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Call the takeDamage method on the enemy
                    if(hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<EnemyAI>())
                    {
                        damageAmount = spell.GetComponent<Mana>().damageamount;
                        hit.collider.GetComponent<EnemyAI>().takeDamage(spell.GetComponent<Mana>().damageamount);
                    }
                    else if(hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<LavaGolemAI>())
                    {
                        damageAmount = spell.GetComponent<Mana>().damageamount;
                        hit.collider.GetComponent<LavaGolemAI>().takeDamage(spell.GetComponent<Mana>().damageamount);
                    }
                    else if (hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<BabLava>())
                    {
                        damageAmount = spell.GetComponent<Mana>().damageamount;
                        hit.collider.GetComponent<BabLava>().takeDamage(spell.GetComponent<Mana>().damageamount);
                    }
                    else if (hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<Destroywall>())
                    {
                        damageAmount = spell.GetComponent<Mana>().damageamount;
                        hit.collider.GetComponent<BabLava>().takeDamage(spell.GetComponent<Mana>().damageamount);
                    }

                }
                else if (hit.collider.CompareTag("Skeleton"))
                {
                    // Call the takeDamage method on the enemy
                    damageAmount = spell.GetComponent<Mana>().damageamount;
                    hit.collider.GetComponent<SkeletonAI>().takeDamage(spell.GetComponent<Mana>().damageamount);
                }
                else if (hit.collider.CompareTag("MovingBox") && hit.collider.GetComponent<ShootingBoxAI>())
                {
                    // Call the takeDamage method on the enemy
                    damageAmount = spell.GetComponent<Mana>().damageamount;
                    hit.collider.GetComponent<ShootingBoxAI>().takeDamage(spell.GetComponent<Mana>().damageamount);
                }
            }

            // Get the spell's script
            Mana spellScript = spellInstance.GetComponent<Mana>();

            // Deduct the mana cost from the player's mana
            mana -= spellScript.Manacost;
            UpdatePlayerMana();

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            // Handle the case where the player doesn't have enough mana
            Debug.Log("Not enough mana to cast the spell!");
        }
    }
   

    IEnumerator DamageFlash()
    {
        gamemanager.instance.PlayerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.PlayerDamageScreen.SetActive(false);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdatePlayerUI();
        StartCoroutine(DamageFlash());

        if (HP <= 0)
        {
            gamemanager.instance.YouLose();
        }
    }

    public void gainHealth(int amount)
    {
        HP += amount;
        UpdatePlayerUI();
    }

    public void gainMana(int amount)
    {
        mana = Mathf.Min(mana + amount, ManaOrig);
        UpdatePlayerMana();
    }

    public void resetHealth()
    {
        HP = HPorig;
    }

    public void resetMana() 
    {
        mana = ManaOrig;
    }

    public void gainExperience(int amount)
    {
        experience += amount;
        UpdatePlayerUI();
        if(experience >= experienceToNextLevel)
        {
            levelUp(); //level up player
        }
    }
    public int GetLevel() { return level; }
    public int GetHealth() { return HP; }
    public int GetMana() { return mana; }

    public int GetMaxHealth() { return HPorig; }

    public int GetMaxMana() { return ManaOrig; }

    public void SetMaxHealth(int maxHealth)
    {
        HPorig = maxHealth;
    }

    public void SetMaxMana(int maxMana)
    {
        ManaOrig = maxMana;
    }

    public void SetMana(int ManaOrig)
    {
        mana = ManaOrig;

    }

    public void SetHealth (int HPOrig)
    {
        HP = HPOrig;
    }

    void levelUp()
    {
        level++;
        experience = 0;
        experienceToNextLevel = calculateExperienceToNextLevel();
        HP = HPorig; //reset health to max
        mana = ManaOrig;
        spell.GetComponent<Mana>().damageamount += 5;
        gamemanager.instance.LevelUp(); //open menu for leveling up
        UpdatePlayerUI();
        Debug.Log("Level Up! You are now level " + level);
    }
  private int calculateExperienceToNextLevel()
    {
        return experienceToNextLevel * 2;
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
        gamemanager.instance.playerXPBar.fillAmount = (float)experience / experienceToNextLevel;
        gamemanager.instance.LevelText.text = level.ToString();
        gamemanager.instance.XpText.text = experience.ToString() + " / " + experienceToNextLevel.ToString();
       
    }

    public void UpdatePlayerMana()
    {
        gamemanager.instance.playerMPBar.fillAmount = (float)mana / ManaOrig;
    }
    public void getWandstats(Wands wand)
    {
        Wandlist.Add(wand);
        selectwandPos = Wandlist.Count - 1;

        spell = wand.Spell;
        shootRate = wand.shootRate;
       
       
    }

    public void getItemStats(ItemData item)
 {
     itemList.Add(item);
     selectItemPos = itemList.Count - 1;
     healedAmount = item.healingAmount;
     manaRestored=item.manaRAmount; ///
     healthBoosted=item.healthBoosted;
     manaBoosted = item.manaBoosted;
     inventoryModel.GetComponent<MeshFilter>().sharedMesh=item .inventoryModel.GetComponent<MeshFilter>().sharedMesh;
     inventoryModel.GetComponent<MeshRenderer>().sharedMaterial=item.inventoryModel.GetComponent<MeshRenderer>().sharedMaterial;
 }

    void selectWand()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectwandPos < Wandlist.Count - 1)
        {
            selectwandPos++;
            
            changeWand();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectwandPos > 0)
        {
            selectwandPos--;
            changeWand();
        }
    }
    void changeWand()
    {
        spell = Wandlist[selectwandPos].Spell;
        shootRate = Wandlist[selectwandPos].shootRate;
        wandModel.GetComponent<MeshFilter>().sharedMesh = Wandlist[selectwandPos].wandModel.GetComponent<MeshFilter>().sharedMesh;
        wandModel.GetComponent<MeshRenderer>().material = Wandlist[selectwandPos].wandModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}

