using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] public int HP;
    [SerializeField] public int mana;
    [SerializeField] public int experience;
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
    public List<ItemData> itemList = new List<ItemData>();

    [SerializeField] GameObject spell;
     public Transform shootPos;
    [SerializeField] public Transform headPos;
    [SerializeField] float shootRate;
    [SerializeField] GameObject wandModel;
    [SerializeField] GameObject inventoryModel;
    [SerializeField] GameObject shieldprefab;
    [SerializeField] float shieldDuration;
    private CoinSystem coinSystem;
    
      
    [SerializeField] float skillCooldownDuration;
   [SerializeField] int healedAmount;
    [SerializeField] int manaRestored;
    [SerializeField] int healthBoosted;
    [SerializeField] int manaBoosted;
    Vector3 moveDir;
    Vector3 playerVel;

    public TextMeshProUGUI currentQuest;
    float skillCooldown;
    GameObject currentShield;
    public string currentQuestName; // Current quest name
    int jumpCount;
    int selectWandPos;
    public int selectItemPos;
    public int HPorig;
    int XPorig;
    public int ManaOrig;
    int origSpeed;
    float damageAmount;
    public TextMeshProUGUI playerQuest;
    bool usingItem;
    bool isSprinting;
    bool isSliding;
    bool isShooting;
    public bool hasQuest;
    public static PlayerController instance;
    public string areaTransitionName;
    void Awake()
    {
        //Ensure that this is the only instance of PlayerController
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject); // Keep this object across scenes
        //}
        //else
        //{
        //    Destroy(gameObject); // Destroy duplicate instance
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
     
       
        DontDestroyOnLoad(gameObject);
        HPorig = HP;
        ManaOrig = mana;
        XPorig = experience;
        //SetMaxHealth(HPorig);
        //SetMaxMana(ManaOrig);
        origSpeed = speed;
        UpdatePlayerUI();
        UpdatePlayerMana();
        // Ensure the CoinSystem reference is assigned
        if (coinSystem == null)
        {
            coinSystem = FindObjectOfType<CoinSystem>(); // Find the CoinSystem in the scene
            if (coinSystem == null)
            {
             
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!gamemanager.instance.isPaused)
        {
            movement();
            selectWand();
            selectItem();
            if (Input.GetButtonDown("UseItem") && !usingItem && itemList.Count > 0)
            {
                StartCoroutine(useItem());
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
        // If the player is a child of the platform, we can use the platform's movement
        if (transform.parent != null && transform.parent.CompareTag("MovingPlatform"))
        {
            // The player will move with the platform, so we don't need to apply movement here
            controller.Move(Vector3.zero); // No additional movement
        }
        else
        {
            // Move the player normally
            controller.Move(moveDir * speed * Time.deltaTime);
        }

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
    
    void ActivateWandAblity()
    {
        Wands currentwand = Wandlist[selectWandPos];
        if (currentwand.skill != null && skillCooldown <= 0)
        {
            currentwand.skill.Activate(this);
            skillCooldown = skillCooldownDuration;
        }
    }

    void ActivateShield()
    {
        if(currentShield == null)
        {
            currentShield = Instantiate(shieldprefab, transform.position, Quaternion.identity);
            currentShield.transform.SetParent(transform);
        }
        Destroy(currentShield, shieldDuration);
    }

    IEnumerator useItem()
    {
        if (Input.GetButtonDown("UseItem") && !usingItem && itemList.Count > 0)
        {
            usingItem = true;
            if (selectItemPos < 0 || selectItemPos >= itemList.Count)
            {
                selectItemPos = 0;
            }
            // Use the item
            ItemData item = itemList[selectItemPos];

            // Apply the item's effects
            if (item.category == ItemCategory.Consumable)
            {
                // Heal the player
                if (healedAmount > 0)
                {
                    gainHealth(healedAmount);
                }

                // Restore mana
                if (manaRestored > 0)
                {
                    gainMana(manaRestored);
                }

                // Remove the item from the list
                itemList.RemoveAt(selectItemPos);
                if (itemList.Count > 0)
                {
                    if (selectItemPos >= itemList.Count)
                    {
                        selectItemPos = itemList.Count - 1;
                    }
                    //Update the list of items to next in list
                    changeItem();
                }
                else
                {
                    // If there are no items left, reset selection
                    inventoryModel.GetComponent<MeshFilter>().sharedMesh = null;
                    inventoryModel.GetComponent<MeshRenderer>().enabled = false;
                    selectItemPos = 0;

                }
                // Adjust the selected item position
                if (selectItemPos >= itemList.Count && itemList.Count > 0)
                {
                    selectItemPos = itemList.Count - 1; // Move to the last item if the current one is removed
                }
                else if (itemList.Count == 0)
                {
                    // If there are no items left, reset selection
                    selectItemPos = 0;
                }
            }

           // Debug.LogWarning("Item " + selectItemPos + "used");
            yield return new WaitForSeconds(1f); // Example cooldown time
            usingItem = false;

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
                    if (hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<EnemyAI>())
                    {
                        damageAmount = spell.GetComponent<Mana>().damageamount;
                        hit.collider.GetComponent<EnemyAI>().takeDamage(spell.GetComponent<Mana>().damageamount);
                    }
                    else if (hit.collider.CompareTag("Enemy") && hit.collider.GetComponent<LavaGolemAI>())
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
        if (experience >= experienceToNextLevel)
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

    public void SetHealth(int HPOrig)
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
    
    }
    private int calculateExperienceToNextLevel()
    {
        return experienceToNextLevel * 2;
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    // Method to load player data from the save system.
    public void LoadSystem()
    {
        // Attempt to load player data using the SaveSystem class.
        PlayerData data = SaveSystem.LoadPlayer();

        // Check if the loaded data is not null (i.e., the load was successful).
        if (data != null)
        {
            // Set the player's level based on the loaded data.
            level = data.level;

            // Set the player's health based on the loaded data.
            HP = data.health;
            experience = data.experience;
            // Create a Vector3 variable to store the player's position.
            Vector3 position;

            // Assign the x-coordinate from the loaded data to the position variable.
            position.x = data.position[0];

            // Assign the y-coordinate from the loaded data to the position variable.
            position.y = data.position[1];

            // Assign the z-coordinate from the loaded data to the position variable.
            position.z = data.position[2];

            // Update the player's transform position in the game world to the loaded position.
            transform.position = position;

            // Load quest data if the player has an active quest.
            if (data.hasQuest)
            {
                // Find the QuestGiver object in the scene to interact with quests.
                QuestGiver questGiver = FindObjectOfType<QuestGiver>();

                // Check if a QuestGiver was found in the scene.
                if (questGiver != null)
                {
                    // Set the current quest name based on the loaded data.
                    currentQuestName = data.currentQuestName;

                    // Indicate that the player has an active quest.
                    hasQuest = true; // This flag can be used to manage quest-related logic.
                }
            }
        }
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
        selectWandPos = Wandlist.Count - 1;

        spell = wand.Spell;
        shootRate = wand.shootRate;
    }
    // Method to retrieve and set the stats of a given item and update the inventory model.
    public void getItemStats(ItemData item)
    {
        // Add the specified item to the itemList, which represents the player's inventory.
        itemList.Add(item);

        // Set the selected item position to the last index of the itemList (the newly added item).
        selectItemPos = itemList.Count - 1;
        // Check if the item is currency
        if (item.category == ItemCategory.Currency)
        {
            coinSystem.Gain(item.currencyValue);
     
        }
        else
        {
            // Retrieve and store the healing amount of the item for later use.
            healedAmount = item.healingAmount;

            // Retrieve and store the mana restoration amount of the item for later use.
            manaRestored = item.manaRAmount; // This line retrieves the mana restoration value.

            // Retrieve and store the health boost value of the item for later use.
            healthBoosted = item.healthBoosted;

            // Retrieve and store the mana boost value of the item for later use.
            manaBoosted = item.manaBoosted;

            // Update the inventory model's mesh to match the selected item's model.
            // This sets the visual representation of the item in the inventory.
            inventoryModel.GetComponent<MeshFilter>().sharedMesh = item.inventoryModel.GetComponent<MeshFilter>().sharedMesh;

            // Update the inventory model's material to match the selected item's material.
            // This ensures that the visual appearance of the item is consistent with its properties.
            inventoryModel.GetComponent<MeshRenderer>().sharedMaterial = item.inventoryModel.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }

    // Method to handle item selection in the inventory based on player input.
    void selectItem()
    {
        // Check if the player has pressed the button to scroll up in the inventory.
        // Ensure that the selected item position is not at the last item in the list.
        if (Input.GetButtonDown("InventoryScrollUp") && selectItemPos < itemList.Count - 1)
        {
            // Increment the selected item position to move to the next item in the inventory.
            selectItemPos++;

            // Call the changeItem method to update the displayed item information.
            changeItem();
        }
        // Check if the player has pressed the button to scroll down in the inventory.
        // Ensure that the selected item position is not at the first item in the list.
        else if (Input.GetButtonDown("InventoryScrollDown") && selectItemPos > 0)
        {
            // Decrement the selected item position to move to the previous item in the inventory.
            selectItemPos--;

            // Call the changeItem method to update the displayed item information.
            changeItem();
        }
    }
    void selectWand()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectWandPos < Wandlist.Count - 1)
        {
            selectWandPos++;

            changeWand();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectWandPos > 0)
        {
            selectWandPos--;
            changeWand();
        }
    }
    void changeItem()
    {
        // Check if the selected item position is out of bounds (negative or greater than the last index of itemList).
        if (selectItemPos < 0 || selectItemPos >= itemList.Count)
        {
            // If the selected position is invalid, reset it to the first item (index 0).
            selectItemPos = 0;
        }

        // Check if there are any items in the itemList.
        if (itemList.Count > 0)
        {
            // Retrieve the healing amount of the selected item from the itemList.
            healedAmount = itemList[selectItemPos].healingAmount;

            // Retrieve the mana restoration amount of the selected item from the itemList.
            manaRestored = itemList[selectItemPos].manaRAmount; // This line retrieves the mana restoration value.

            // Retrieve the health boost value of the selected item from the itemList.
            healthBoosted = itemList[selectItemPos].healthBoosted;

            // Retrieve the mana boost value of the selected item from the itemList.
            manaBoosted = itemList[selectItemPos].manaBoosted;

            // Update the inventory model's mesh to match the selected item's model.
            // This sets the visual representation of the item in the inventory.
            inventoryModel.GetComponent<MeshFilter>().sharedMesh = itemList[selectItemPos].inventoryModel.GetComponent<MeshFilter>().sharedMesh;

            // Update the inventory model's material to match the selected item's material.
            // This ensures that the visual appearance of the item is consistent with its properties.
            inventoryModel.GetComponent<MeshRenderer>().sharedMaterial = itemList[selectItemPos].inventoryModel.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else
        {
            // If there are no items in the itemList, disable the inventory model's mesh renderer.
            // This hides the inventory model since there are no items to display.
            inventoryModel.GetComponent<MeshRenderer>().enabled = false;

            // Set the inventory model's mesh to null, effectively clearing the visual representation.
            inventoryModel.GetComponent<MeshFilter>().sharedMesh = null;
        }
    }


    void changeWand()
    {
        spell = Wandlist[selectWandPos].Spell;
        shootRate = Wandlist[selectWandPos].shootRate;
        wandModel.GetComponent<MeshFilter>().sharedMesh = Wandlist[selectWandPos].wandModel.GetComponent<MeshFilter>().sharedMesh;
        wandModel.GetComponent<MeshRenderer>().material = Wandlist[selectWandPos].wandModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    // Method to add an item to the inventory.
    public void AddItemInventory(ItemData item)
    {
        // Check if the item being added is not null (i.e., it exists).
        if (item != null)
        {
            // Add the item to the itemList, which represents the player's inventory.
            itemList.Add(item);

            // Log a message to the console indicating that the item has been added to the inventory.
  
        }
    }

    // Method to remove an item from the inventory.
    public void RemoveItemInventory(ItemData item)
    {
        // Attempt to remove the specified item from the itemList.
        // The Remove method returns true if the item was successfully removed.
        if (itemList.Remove(item))
        {if (item.category == ItemCategory.Currency)
            {
                coinSystem.RemoveCurrency(item);
            }
         
        }
    }
}
