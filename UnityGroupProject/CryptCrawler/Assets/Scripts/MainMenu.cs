using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;
    string newGameScene = "CryptCrawler";
    public void Start()
    {
        int highScore= SaveLaodManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Score Survived: " + highScore;
    }


    public void startNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit(); 
#endif

    }
}
