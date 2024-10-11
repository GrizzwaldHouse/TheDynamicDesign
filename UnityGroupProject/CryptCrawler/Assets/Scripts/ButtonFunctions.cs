using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpause();

    }
    //public void useHealthPotion()
    //{
    //    gamemanager.instance.playerHealth += 10;
    //    gamemanager.instance.updatePlayerUI();
    //}
    //public void useManaPotion()
    //{
    //    gamemanager.instance.playerMana += 10;
    //    gamemanager.instance.updatePlayerUI();
    //}
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
