using JetBrains.Annotations;
using SkeletonEditor;
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
    [SerializeField] GameObject menuLose;// The lose menu game object
    [SerializeField] GameObject menuSettings;
    [SerializeField] public GameObject creditsPanel;
    [SerializeField] public GameObject questMenu;
    public GameObject PlayerDamageScreen;
    public LavaGolemAI boss;
    public Button saveButton;
    public Button loadButton;
    public GameObject inventoryUI;
    public GameObject playerSpawnPos;



    // This variable is exposed to the Unity Inspector, allowing designers to assign the text component that displays the enemy count.
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] public TMP_Text LevelText;
    [SerializeField] public TMP_Text XpText;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] TextMeshProUGUI QuestProgress;

    // This variable is exposed to the Unity Inspector, allowing designers to assign the player game object.
    public GameObject player;

    // Player's hp bar.
    public Image playerHPBar;
    public Image playerMPBar;
    public Image playerXPBar;
    // This is a singleton instance of the GameManager, allowing other scripts to access it easily.
    public static gamemanager instance;
    public PlayerController accessPlayer;
   // public QuestGiver accessQuestGiver;
    public SectionTrigger accessSectionTrigger;
    // New variables to track objectives
    [SerializeField] int totalObjectives ; // Set this to the total number of objectives in your game
    [SerializeField] int completedObjectives;
    [SerializeField] TMP_Text objectivesText; // Reference to the UI Text for displaying objectives
   
    private int totalDestroyedObjects = 0; // Track the total number of destroyed objects
    [SerializeField] private int questAmount; // Number of objects to destroy to disable the spawner

    // This flag indicates whether the game is currently paused.
    public bool isPaused;
    
    // This variable stores the original time scale, which is restored when the game is unpaused.
    float timeScaleOrig;

    // This variable keeps track of the current enemy count.
    int enemyCount;
    int level;
    // Reference to the ObjectSpawner
    [SerializeField] private ObjectSpawner objectSpawner; // Add this line

    // This method is called once at the start of the game.
    void Awake()
    {
        // Set the singleton instance of the GameManager to this script.
        instance = this;

        // Store the original time scale.
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        accessPlayer = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
       
        

    }
    void Start()
    {
        //Initialize the buttons
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        // Initialize the objectives UI
       UpdateObjectivesUI();
        

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
        if (Input.GetButtonUp("Quest"))
        {
            if (questMenu.activeSelf)
            {
                CloseQuest();
            }
            else
            {
                OpenQuest();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && creditsPanel.activeSelf)
        {
            creditsPanel.SetActive(false); 
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
        if (boss.HP <= 0)
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
        // Call ResetSpawner to reset the spawner state
        if (objectSpawner != null)
        {
            objectSpawner.ResetSpawner();
        }
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
        // Call the SavePlayer method in the SaveSystem.
        SaveSystem.SavePlayer(accessPlayer); // Pass the PlayerController to save its state


    
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

    public void OpenQuest()
    {
        questMenu.SetActive(true);
        if (accessPlayer.hasQuest == true)
        {
            questText.text = accessPlayer.currentQuest.text;
        }
        else
        {
            questText.text = "No Current Quests!";
        }
    }

    public void CloseQuest()
    {
        questMenu.SetActive(false);
    }

    // This method loads the game by calling the LoadSystem method on the PlayerController.
    public void LoadGame()
    {
     
        //// Call the LoadSystem method on the PlayerController.
        accessPlayer.LoadSystem();

      
    }

    public void CompleteObjective()
    {
        // Increment the completed objectives count
        completedObjectives++;

        // Update the UI
        UpdateObjectivesUI();

        // Check if all objectives are completed
        if (completedObjectives >= totalObjectives)
        {
            // Call the method to handle quest completion
            QuestCompleted();
        }
    }

    private void UpdateObjectivesUI()
    {
        // Update the UI text to show remaining and completed objectives
        objectivesText.text = $"Objectives: {completedObjectives}/{totalObjectives}";
    }
    private void QuestCompleted()
    {
      
    }

    public void ToggleCredits()
    {
        creditsPanel.SetActive(true);
    }
    
}