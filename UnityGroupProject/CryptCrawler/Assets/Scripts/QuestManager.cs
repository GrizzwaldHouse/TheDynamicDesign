using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, IQuestManager
{
    // List of ObjectSpawners to control
    [SerializeField]
    private List<ObjectSpawner> objectSpawners; // List of spawners to manage

public void AddSpawner(ObjectSpawner spawner)
    {
        // Implementation for adding a spawner
        objectSpawners.Add(spawner);
    }

    // Method to be called when the quest is completed
    public void CompleteQuest()
    {
        // Notify all ObjectSpawners to disable their triggers and destroy spawned objects
        foreach (var spawner in objectSpawners)
        {
            // Check if the spawner is not null
            if (spawner != null)
            {
                spawner.DisableSpawner(); // Disable the spawner's trigger
                spawner.DestroyAllSpawnedObjects(); // Destroy all objects spawned by this spawner
            }
        }
        // Call the CompleteObjective method from the gamemanager
        gamemanager.instance.CompleteObjective();
    }

    public void RemoveSpawner(ObjectSpawner spawner)
    {
        // Implementation for removing a spawner
        objectSpawners.Remove(spawner);
    }
}
