using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This attribute is used to indicate that the PlayerData class can be serialized.
// Serialization is the process of converting an object's state to a format that can be written to a file or transmitted over a network.
[System.Serializable]
public class PlayerData
{
    // This variable stores the player's current level.
    // It's a public integer, which means it can be accessed and modified from outside the class.
    public int level;

    // This variable stores the player's current health.
    // It's a public integer, which means it can be accessed and modified from outside the class.
    public int health;

    // This variable stores the player's current position in 3D space.
    // It's a public array of floats, which means it can be accessed and modified from outside the class.
    // The array has three elements, representing the x, y, and z coordinates of the player's position.
    public float[] position;
    public string currentQuestName; // New field for the current quest name
    public bool hasQuest; // New field to indicate if the player has a quest
    public int experience; // New field to indicate if the player has an experience
    // This is a constructor for the PlayerData class.
    // A constructor is a special method that's called when an object is created from the class.
    // This constructor takes a PlayerController object as a parameter, which is used to initialize the PlayerData object's variables.
    public PlayerData(PlayerController player)
    {
        // This line sets the level variable to the player's current level.
        // The GetLevel method is called on the PlayerController object to get the player's current level.
        level = player.GetLevel();

        // This line sets the health variable to the player's current health.
        // The GetHealth method is called on the PlayerController object to get the player's current health.
        health = player.GetHealth();

        // This line creates a new array of floats to store the player's position.
        // The array has three elements, representing the x, y, and z coordinates of the player's position.
        position = new float[3];

        // These lines set the x, y, and z coordinates of the player's position.
        // The transform.position property of the PlayerController object is used to get the player's current position.
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        currentQuestName = player.currentQuestName; // Save current quest name
        hasQuest = player.hasQuest; // Save whether the player has a quest
        experience = player.experience; // Save player experience
    }

}