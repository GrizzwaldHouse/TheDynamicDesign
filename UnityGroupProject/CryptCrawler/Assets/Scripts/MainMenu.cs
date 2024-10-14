//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MainMenu : MonoBehaviour
//{
//    // Reference to the TextMeshProUGUI component that displays the high score
//    public TMP_Text highScoreUI;

//    // The name of the scene to load when starting a new game
//    string newGameScene = "CryptCrawler";

//    // Called when the script is initialized
//    public void Start()
//    {
//        // Load the high score from the SaveLoadManager instance
//        int highScore = SaveLoadManager.Instance.LoadHighScore();

//        // Update the high score UI text with the loaded high score
//        highScoreUI.text = $"Top Score Survived: " + highScore;
//    }

//    // Called when the "Start New Game" button is clicked
//    public void startNewGame()
//    {
//        // Load the new game scene using the SceneManager
//        SceneManager.LoadScene(newGameScene);
//    }

//    // Called when the "Exit" button is clicked
//    public void ExitApplication()
//    {
//        // If we're in the Unity Editor, stop playing the scene
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;

//#else
//        // If we're in a built application, quit the application
//        Application.Quit(); 
//#endif
//    }
//}