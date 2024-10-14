using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enum for the different types of potions
public enum PotionType { HealthPotion, ManaPotion }

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
        if (potionType == PotionType.HealthPotion)
        {
            

            targetScript.gainHealth(healthToGain);

            // Destroy the potion object
            Destroy(gameObject);

        }
        else if (potionType == PotionType.ManaPotion)
        {
            //TO DO add mana addition code

            Destroy(gameObject);
        }


       
    }
}