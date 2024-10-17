using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Define an enum for the different types of potions
public enum PotionType { HealthPotion, ManaPotion, PowerBuff, ManaBuff }

// Define an interface for potions
public interface IPotion
{
    // No methods are defined in this interface, but it can be used to define a contract for potions
}

// Define a class for potions
public class Potions : MonoBehaviour, IInteractable
{
    // Define a variable to store the type of potion
    public PotionType potionType;
    public PlayerController targetScript;


    [SerializeField] int healthToGain;
    [SerializeField] int manaToGain;
    [SerializeField] int potionRespawnRate;

    GameObject HealthPotion;
    GameObject ManaPotion;

      public void Interact()
    {
        
        //Check the type of potion and apply its effect to the player
        switch (potionType)
        {
            case PotionType.HealthPotion:
                targetScript.gainHealth(healthToGain);
                Destroy(gameObject);
                break;

            case PotionType.ManaPotion:
                //Add code for ManaPotion
                targetScript.gainMana(manaToGain);
                Destroy(gameObject);
                break;

            case PotionType.PowerBuff:
                //Add code for PowerBuff
                break;

            case PotionType.ManaBuff:
                //Add code for ManaBuff
                break;
        }
       
    }
}