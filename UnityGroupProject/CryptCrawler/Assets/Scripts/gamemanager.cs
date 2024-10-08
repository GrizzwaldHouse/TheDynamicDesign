using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// This script is responsible for managing the game's state and UI.
public class gamemanager : MonoBehaviour
{
    // These variables are exposed to the Unity Inspector, allowing designers to assign the corresponding game objects.
    [SerializeField] GameObject menuActive; // The currently active menu (e.g. main menu, pause menu, win menu, lose menu)
    [SerializeField] GameObject menuPause; // The pause menu game object
    [SerializeField] GameObject menuWin; // The win menu game object
    [SerializeField] GameObject menuLose; // The lose menu game object
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text manaText;
    // This variable is exposed to the Unity Inspector, allowing designers to assign the text component that displays the enemy count.
    [SerializeField] TMP_Text enemyCountText;
    public int playerHealth;
    public int playerMana;
    [SerializeField] GameObject healthPotionPrefab;
    [SerializeField] GameObject manaPotionPrefab;
    // This variable is exposed to the Unity Inspector, allowing designers to assign the player game object.
    public GameObject player;

    // This is a singleton instance of the GameManager, allowing other scripts to access it easily.
    public static gamemanager instance;

    // This flag indicates whether the game is currently paused.
    public bool isPaused;

    // This variable stores the original time scale, which is restored when the game is unpaused.
    float timeScaleOrig;

    // This variable keeps track of the current enemy count.
    int enemyCount;

    // This method is called once at the start of the game.
    void Start()
    {
        // Set the singleton instance of the GameManager to this script.
        instance = this;

        // Store the original time scale.
        timeScaleOrig = Time.timeScale;
    }

    // This method is called every frame.
    void Update()
    {
        // Check if the "Cancel" button (e.g. Esc key) is pressed.
        if (Input.GetButtonDown("Cancel"))
        {
            // If the menu is not active, pause the game and show the pause menu.
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            // If the pause menu is active, unpause the game.
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }
    public void updatePlayerUI()
    {
        healthText.text = "Health: " + playerHealth;
        manaText.text = "Mana: " + playerMana;

    }
    // This method pauses the game and shows the pause menu.
    public void statePause()
    {
        // Toggle the pause flag.
        isPaused = !isPaused;

        // Set the time scale to 0, effectively pausing the game.
        Time.timeScale = 0;

        // Show the cursor and confine it to the game window.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // This method unpauses the game and hides the pause menu.
    public void stateUnpause()
    {
        // Toggle the pause flag.
        isPaused = !isPaused;

        // Restore the original time scale.
        Time.timeScale = timeScaleOrig;

        // Hide the cursor and lock it to the game window.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Deactivate the pause menu and reset the active menu to null.
        menuActive.SetActive(false);
        menuActive = null;
    }

    // This method updates the enemy count and displays it on the UI.
    public void updateGameGoal(int amount)
    {
        // Update the enemy count.
        enemyCount += amount;

        // Update the UI text to display the new enemy count.
        enemyCountText.text = enemyCount.ToString("F0");

        // If the enemy count reaches 0, pause the game and show the win menu.
        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    // This method is called when the player loses, pausing the game and showing the lose menu.
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
        public void spawnHealthPotion()
        {
            GameObject healthPotion = Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            healthPotion.GetComponent<Potions>().setPotionType(PotionType.HealthPotion);
            // This method spawns a health potion at the current position and sets its type to HealthPotion.
        }

        public void spawnManaPotion()
        {
            GameObject manaPotion = Instantiate(manaPotionPrefab, transform.position, Quaternion.identity);
            manaPotion.GetComponent<Potions>().setPotionType(PotionType.ManaPotion);
            // This method spawns a mana potion at the current position and sets its type to Mana.
        }

        public void useHealthPotion()
        {
            playerHealth += 10;
            updatePlayerUI();
            // This method increases the player's health by 10 and updates the UI to reflect the change.
        }

        public void useManaPotion()
        {
            playerMana += 10;
            updatePlayerUI();
            // This method increases the player's mana by 10 and updates the UI to reflect the change.
        }
    }
