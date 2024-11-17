using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// This class is responsible for saving and loading player data.
// It inherits from MonoBehaviour, which means it can be attached to a GameObject in the Unity scene.
public class SaveSystem : MonoBehaviour
{
   private PlayerData playerData;
    public static SaveSystem instance;
    // This method saves the player's data to a file.
    // It's a static method, which means it can be called without creating an instance of the SaveSystem class.
    public static void SavePlayer(PlayerController player)
    {
        // This line creates a new BinaryFormatter object, which is used to serialize the player's data.
        BinaryFormatter formatter = new BinaryFormatter();

        // This line constructs the path to the save file.
        // Application.persistentDataPath is a Unity property that returns the path to the persistent data directory.
        // The "/player.StaffOnly" part is the name of the save file.
        string path = Application.persistentDataPath + "/player.StaffOnly";

        // This line creates a new FileStream object, which is used to write to the save file.
        // FileMode.Create means that the file will be created if it doesn't exist, and overwritten if it does.
        FileStream stream = new FileStream(path, FileMode.Create);

        // This line creates a new PlayerData object, which is used to store the player's data.
        // The PlayerData object is initialized with the player's current data.
        PlayerData data = new PlayerData(player);

        // This line serializes the PlayerData object and writes it to the save file.
        formatter.Serialize(stream, data);

        // This line closes the FileStream object, which is necessary to ensure that the data is written to the file.
        stream.Close();
    }

    // This method loads the player's data from a file.
    // It's a static method, which means it can be called without creating an instance of the SaveSystem class.
    public static PlayerData LoadPlayer()
    {
        // This line constructs the path to the save file.
        // Application.persistentDataPath is a Unity property that returns the path to the persistent data directory.
        // The "/player.StaffOnly" part is the name of the save file.
        string path = Application.persistentDataPath + "/player.StaffOnly";

        // This line checks if the save file exists.
        if (File.Exists(path))
        {
            // This line creates a new BinaryFormatter object, which is used to deserialize the player's data.
            BinaryFormatter formatter = new BinaryFormatter();

            // This line creates a new FileStream object, which is used to read from the save file.
            // FileMode.Open means that the file will be opened for reading.
            FileStream stream = new FileStream(path, FileMode.Open);

            // This line deserializes the player's data from the save file and stores it in a PlayerData object.
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            // This line closes the FileStream object, which is necessary to ensure that the data is read from the file.
            stream.Close();

            // This line returns the loaded PlayerData object.
            return data;
        }

        // If the save file doesn't exist, this line logs an error message and returns null.
        else
        {
           // Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public void NewGame(PlayerController player)
    {
        playerData = new PlayerData(player);
    }

    }

