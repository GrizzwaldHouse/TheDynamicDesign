using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLaodManager : MonoBehaviour
{
    public static SaveLaodManager Instance { get; set; }
   string highScoreKey = "BestWaveSavedVale";
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey, score);
    }
    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey)){ 
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }

    }
}
