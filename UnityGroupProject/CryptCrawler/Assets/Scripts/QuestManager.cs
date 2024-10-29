using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

// Class representing a quest in the game
public class Quest
{
    // Property to store the name of the quest, accessible only within this class
    public string QuestName { get; private set; }

    // Property to store a description of the quest, accessible only within this class
    public string Description { get; private set; }

    // Property to indicate whether the quest is completed, accessible only within this class
    public bool IsCompleted { get; private set; }

    // List to hold references to ObjectSpawner instances associated with this quest
    public List<ObjectSpawner> Spawners { get; private set; }
    private List<GameObject> spawnedObjects;

    // Constructor to initialize a new quest with a name and description
    public Quest(string questName, string description)
    {
        QuestName = questName; // Set the quest name
        Description = description; // Set the quest description
        IsCompleted = false; // Initialize the quest as not completed
        Spawners = new List<ObjectSpawner>(); // Initialize the list of spawners
        spawnedObjects = new List<GameObject>(); // Initialize the list of spawned objects
    }

    // Method to mark the quest as completed
    public void CompleteQuest()
    {
        IsCompleted = true; // Set the IsCompleted property to true
        // Additional logic for quest completion can be added here (e.g., rewards)
    }
    // Method to track a spawned object
    public void TrackSpawnedObject(GameObject spawnedObject)
    {
        if (spawnedObject != null)
        {
            spawnedObjects.Add(spawnedObject); // Add the spawned object to the list
        }
    }

    // Method to check if the quest is complete
    public bool CheckCompletionCriteria()
    {
        // Example criteria: Check if a certain number of objects have been spawned
        return spawnedObjects.Count >= 5; // Example: Quest is complete if 5 objects are spawned
    }

}

// Class to manage quests in the game, inheriting from MonoBehaviour for Unity integration
public class QuestManager : MonoBehaviour, IQuestManager
{
    // List of ObjectSpawners to control, exposed to the Unity Inspector
    [SerializeField]  private List<ObjectSpawner> objectSpawners; // List of spawners to manage

    // List of quests, exposed to the Unity Inspector
    [SerializeField]
    private List<Quest> quests; // List of quests

    // Method to create a new quest and add it to the quests list
    public void CreateQuest(string questName, string description)
    {
        Quest newQuest = new Quest(questName, description); // Instantiate a new Quest object
        quests.Add(newQuest); // Add the new quest to the quests list
    }
    public void NotifyObjectSpawned(ObjectSpawner spawner)
    {
        // Logic to handle when an object is spawned by the spawner
        foreach (var quest in quests)
        {
            // Check if the quest has this spawner in its list of spawners
            if (quest.Spawners.Contains(spawner))
            {
              
                // Assuming the quest has a method to track spawned objects
                quest.TrackSpawnedObject(spawner.GetLastSpawnedObject());

                // Example logic: Check if the quest is now complete
                if (quest.CheckCompletionCriteria())
                {
                    quest.CompleteQuest(); // Mark the quest as completed
                                           
                    Debug.Log($"Quest '{quest.QuestName}' has been completed!");
                }
            }
        }
    }
    // Method to complete a specific quest
    public void CompleteQuest(Quest quest)
    {
        // Check if the quest is in the list and is not already completed
        if (quests.Contains(quest) && !quest.IsCompleted)
        {
            quest.CompleteQuest(); // Mark the quest as completed
            CompleteQuest(); // Call the existing CompleteQuest method to handle spawners
        }
    }

    // Method to add a new spawner to the objectSpawners list
    public void AddSpawner(ObjectSpawner spawner)
    {
        // Implementation for adding a spawner
        objectSpawners.Add(spawner); // Add the spawner to the list of object spawners
    }

    // Method to be called when the quest is completed
    public void CompleteQuest()
    {
        // Notify all ObjectSpawners to disable their triggers and destroy spawned objects
        foreach (var spawner in objectSpawners) // Iterate through each spawner in the list
        {
            // Check if the spawner is not null to avoid null reference exceptions
            if (spawner != null)
            {
                spawner.DisableSpawner(); // Disable the spawner's trigger to stop spawning
                spawner.DestroyAllSpawnedObjects(); // Destroy all objects that were spawned by this spawner
            }
        }
        // Call the CompleteObjective method from the GameManager to notify that an objective is completed
        gamemanager.instance.CompleteObjective();
    }

    // Method to remove a spawner from the objectSpawners list
    public void RemoveSpawner(ObjectSpawner spawner)
    {
        // Implementation for removing a spawner
        objectSpawners.Remove(spawner); // Remove the specified spawner from the list
    }
}