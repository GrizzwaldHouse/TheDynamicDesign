using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestManager
{
    void CreateQuest(string questName, string description);
    // Method to complete a quest
    void CompleteQuest();

    // Method to add a spawner to the quest manager
    void AddSpawner(ObjectSpawner spawner);

    // Method to remove a spawner from the quest manager
    void RemoveSpawner(ObjectSpawner spawner);
    void NotifyObjectSpawned(ObjectSpawner spawner); // Define the method

}