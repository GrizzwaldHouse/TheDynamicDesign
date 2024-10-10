using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // A static reference to the SaveLoadManager instance, allowing it to be accessed from other scripts
    public static SaveLoadManager Instance { get; set; }

    // The key used to store the high score in PlayerPrefs
    string highScoreKey = "BestWaveSavedValue";

    // Called when the script is initialized
    private void Awake()
    {
        // Check if an instance of SaveLoadManager already exists
        if (Instance != null && Instance != this)
        {
            // If an instance already exists, destroy this one to prevent duplicates
            Destroy(gameObject);
        }
        else
        {
            // If no instance exists, set this one as the instance
            Instance = this;
        }

        // Prevent the SaveLoadManager from being destroyed when the scene changes
        DontDestroyOnLoad(this);
    }

    // Saves the high score to PlayerPrefs
    public void SaveHighScore(int score)
    {
        // Use PlayerPrefs.SetInt to store the high score with the specified key
        PlayerPrefs.SetInt(highScoreKey, score);
    }

    // Loads the high score from PlayerPrefs
    public int LoadHighScore()
    {
        // Check if the high score key exists in PlayerPrefs
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            // If the key exists, return the stored high score
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            // If the key does not exist, return a default value of 0
            return 0;
        }
    }
}