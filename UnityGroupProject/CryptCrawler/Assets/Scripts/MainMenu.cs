using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
 
    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;
    List<AsyncOperation> sceneToLoad =new List<AsyncOperation>();
   

public void StartGame()
    {
        HideMenu();
        ShowLoadingScreen();
        sceneToLoad.Add(SceneManager.LoadSceneAsync("MainMenu"));
        sceneToLoad.Add(SceneManager.LoadSceneAsync("CryptCrawler", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }
    public void HideMenu()
    {
        menu.SetActive(false);
    }
    public void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }
    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for (int i = 0; i < sceneToLoad.Count; i++)
        {

            while (!sceneToLoad[i].isDone)
            {
                totalProgress += sceneToLoad[i].progress;
                loadingProgressBar.fillAmount = totalProgress / sceneToLoad.Count;
                yield return null;
            }
        }
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}