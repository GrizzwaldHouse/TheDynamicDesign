using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Define an enum for the different types of potions
public enum PotionType { HealthPotion, ManaPotion, HealthBuff, ManaBuff }

// Define a class for potions
public class Potions : MonoBehaviour
{
    // Define a variable to store the type of potion
    public PotionType potionType;
    public PlayerController targetScript;


    [SerializeField] int healthToGain;
    [SerializeField] int manaToGain;
    [SerializeField] int healthBuff;
    [SerializeField] int manaBuff;
    [SerializeField] int buffTimer;
    [SerializeField] GameObject potText;


    GameObject HealthPotion;
    GameObject ManaPotion;
    GameObject HealthBuff;
    GameObject ManaBuff;

    int tempHealthBuff;
    int tempManaBuff;
    int originalHP;
    int originalMP;
    bool itemIsPickedUp;

    IEnumerator MPBuff()
    {
        // Store the original max mana
        originalMP = targetScript.GetMaxMana();

        // Apply the mana buff
        tempManaBuff = originalMP + manaBuff;
        targetScript.SetMaxMana(tempManaBuff);

        // Ensure the player gains mana up to the new maximum
        targetScript.gainMana(tempManaBuff);

        // Update the UI to reflect the new mana values
        targetScript.UpdatePlayerMana();

        // Wait for the buff duration
        yield return new WaitForSeconds(buffTimer);

        // Revert max mana back to original and reset mana
        targetScript.SetMaxMana(originalMP);
        targetScript.resetMana();

        // Update the UI again after resetting the buff
        targetScript.UpdatePlayerMana();

        Destroy(gameObject);
    }
        
    
    IEnumerator HPBuff()
    {
        // Store the original max health
        originalHP = targetScript.GetMaxHealth();

        // Apply the health buff
        tempHealthBuff = originalHP + healthBuff;
        targetScript.SetMaxHealth(tempHealthBuff);

        // Ensure the player gains health up to the new maximum
        targetScript.gainHealth(tempHealthBuff);

        // Update the UI to reflect the new health values
        targetScript.UpdatePlayerUI();


        // Wait for the buff duration
        yield return new WaitForSeconds(buffTimer);

        // Revert max health back to original and reset health
        targetScript.SetMaxHealth(originalHP);
        targetScript.resetHealth();

        // Update the UI again after resetting the buff
        targetScript.UpdatePlayerUI();

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !itemIsPickedUp)
        {
            itemIsPickedUp = true;
            gamemanager.instance.accessPlayer.gainHealth(healthToGain);
            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            potText.SetActive(false);
        }
    }

}