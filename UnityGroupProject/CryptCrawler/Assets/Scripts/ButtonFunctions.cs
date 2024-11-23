using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject creditsPanel;
    public ShopManager shopManager;
    public GameObject settingsmenu;
    public int itemIndex;
    public bool isSellButton;
    public void OnButtonClick()
    {
       
        // Check if the current button is designated as the "Sell" button.
        // The boolean variable 'isSellButton' determines the action to be taken.
        if (isSellButton)
        { 
            // If it is a sell button, call the SellItem method on the shopManager.
          // Pass the itemIndex to specify which item to sell.
          // This will trigger the selling process for the specified item.
            shopManager.SellItem(itemIndex);
        }
        else
        {
            // If it is not a sell button, it is assumed to be a "Buy" button.
            // Call the BuyItem method on the shopManager.
            // Again, pass the itemIndex to indicate which item to buy.
            // This will initiate the buying process for the specified item.
            shopManager.BuyItem(itemIndex);
        } 
    }
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
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
    public void BoostHP()
    {
        gamemanager.instance.accessPlayer.HP += 20;
        gamemanager.instance.accessPlayer.HPorig = gamemanager.instance.accessPlayer.HP;
        gamemanager.instance.stateUnpause();
    }
    public void BoostMana()
    {
        gamemanager.instance.accessPlayer.mana += 20;
        gamemanager.instance.accessPlayer.ManaOrig = gamemanager.instance.accessPlayer.mana;
        gamemanager.instance.stateUnpause();
    }

    public void ToggleCredits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
    public void Volume()
    {
        settingsmenu.SetActive(true);
        
    }
    public void Back()
    {
        settingsmenu.SetActive(false);
    }
}
