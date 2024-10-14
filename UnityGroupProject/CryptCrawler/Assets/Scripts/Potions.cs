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
public class Potions : MonoBehaviour
{
    // Define a variable to store the type of potion
    public PotionType potionType;

    // Define a method to set the type of potion
    //public void setPotionType(PotionType pType)
    //{
    //    // Set the potion type to the specified value
    //    this.potionType = pType;
    //}

    // Define a method to handle collisions with other objects
    public void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player"))
        {

            {
                // Check the type of potion and apply its effect to the player
                if (potionType == PotionType.HealthPotion)
                {
                    //// Increase the player's health by 10
                    //gamemanager.instance.playerHealth += 10;
                    // Update the player's UI to reflect the change
                    gamemanager.instance.updatePlayerUI();
                }
                else if (potionType == PotionType.ManaPotion)
                {
                    //// Increase the player's mana by 10
                    //gamemanager.instance.playerMana += 10;
                    // Update the player's UI to reflect the change
                    gamemanager.instance.updatePlayerUI();
                }
                // Destroy the potion object
                Destroy(gameObject);
            }
        }
    }
}