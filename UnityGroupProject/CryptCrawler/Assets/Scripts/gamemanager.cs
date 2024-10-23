using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// This script is responsible for managing the game's state and UI.
public class gamemanager : MonoBehaviour
{
    // These variables are exposed to the Unity Inspector, allowing designers to assign the corresponding game objects.
    [SerializeField] public GameObject menuActive; // The currently active menu (e.g. main menu, pause menu, win menu, lose menu)
    [SerializeField] GameObject menuPause; // The pause menu game object
    [SerializeField] GameObject LevelMenu; //Menu for leveling up
    [SerializeField] GameObject menuWin; // The win menu game object
    [SerializeField] GameObject menuLose; // The lose menu game object
    public GameObject PlayerDamageScreen;
    public LavaGolemAI boss;
    public PlayerController playerScript;
    public Button saveButton;
    public Button loadButton;
    public GameObject inventoryUI;



    // This variable is exposed to the Unity Inspector, allowing designers to assign the text component that displays the enemy count.
    [SerializeField] TMP_Text enemyCountText;

    // This variable is exposed to the Unity Inspector, allowing designers to assign the player game object.
    public GameObject player;
    // Player's hp bar.
    public Image playerHPBar;
    public Image playerMPBar;
    public Image playerXPBar;
    // This is a singleton instance of the GameManager, allowing other scripts to access it easily.
    public static gamemanager instance;
    public PlayerController accessPlayer;

    // This flag indicates whether the game is currently paused.
    public bool isPaused;

    // This variable stores the original time scale, which is restored when the game is unpaused.
    float timeScaleOrig;

    // This variable keeps track of the current enemy count.
    int enemyCount;
    // This method is called once at the start of the game.
    void Awake()
    {
        // Set the singleton instance of the GameManager to this script.
        instance = this;

        // Store the original time scale.
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }
    void Start()
    {
        // Initialize the buttons
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);

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
        if (Input.GetButtonUp("Inventory"))
        {
            if (inventoryUI.activeSelf)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
    public void updatePlayerUI()
    {


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
    public void UpdateGameGoal(int amount)
    {
        // Increment or decrement the enemy count based on the provided amount.
        enemyCount += amount;

        // Update the UI text to display the new enemy count, formatting it as an integer with no decimal places.
        enemyCountText.text = enemyCount.ToString("F0");

        // If the enemy count reaches 0 or less, pause the game and show the win menu.
        if (boss.HP <= 0 )
        {
            // Call the statePause method to pause the game.
            statePause();

            // Set the active menu to the win menu.
            menuActive = menuWin;

            // Activate the win menu.
            menuActive.SetActive(true);
        }
    }

    // This method is called when the player loses, pausing the game and showing the lose menu.
    public void YouLose()
    {
        // Call the statePause method to pause the game.
        statePause();

        // Set the active menu to the lose menu.
        menuActive = menuLose;

        // Activate the lose menu.
        menuActive.SetActive(true);
    }

    public void LevelUp()
    {
        statePause();

        menuActive = LevelMenu;

        menuActive.SetActive(true);
    }

    // This method saves the game by calling the SavePlayer method on the PlayerController.
    public void SaveGame()
    {
        // Find the PlayerController script in the scene.
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        // Call the SavePlayer method on the PlayerController.
        player.SavePlayer();

        // Log a message to the console to indicate that the game has been saved.
        Debug.Log("Game saved!");
    }

    // This method opens the inventory UI.
    public void OpenInventory()
    {
        // Activate the inventory UI.
        inventoryUI.SetActive(true);
    }

    // This method closes the inventory UI.
    public void CloseInventory()
    {
        // Deactivate the inventory UI.
        inventoryUI.SetActive(false);
    }

    // This method loads the game by calling the LoadSystem method on the PlayerController.
    public void LoadGame()
    {
        // Find the PlayerController script in the scene.
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        // Call the LoadSystem method on the PlayerController.
        player.LoadSystem();

        // Log a message to the console to indicate that the game has been loaded.
        Debug.Log("Game loaded!");
    }
}