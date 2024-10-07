using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for controlling the enemy AI and handling damage.
public class enemyAI : MonoBehaviour, IDamage
{
    // This variable is exposed to the Unity Inspector, allowing designers to assign the renderer component of the enemy model.
    [SerializeField] Renderer model;

    // This variable is exposed to the Unity Inspector, allowing designers to set the initial health points (HP) of the enemy.
    [SerializeField] int HP;

    // This variable stores the original color of the enemy model.
    Color colorOrig;

    // This method is called once at the start of the game.
    void Start()
    {
        // Store the original color of the enemy model.
        colorOrig = model.material.color;

        // Notify the game manager that a new enemy has spawned, incrementing the enemy count.
        gamemanager.instance.updateGameGoal(1);
    }

    // This method is called every frame, but is currently empty.
    void Update()
    {
        // TO DO: Add AI logic here, such as movement, attack, or patrol behaviors.
    }

    // This method is called when the enemy takes damage.
    public void takeDamage(int amount)
    {
        // Subtract the damage amount from the enemy's HP.
        HP -= amount;

        // Start a coroutine to flash the enemy's color to indicate damage.
        StartCoroutine(flashColor());

        // If the enemy's HP reaches 0 or less, destroy the enemy game object and notify the game manager.
        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    // This coroutine flashes the enemy's color to indicate damage.
    IEnumerator flashColor()
    {
        // Set the enemy model's color to red to indicate damage.
        model.material.color = Color.red;

        // Wait for 0.1 seconds to create a brief flash effect.
        yield return new WaitForSeconds(0.1f);

        // Restore the enemy model's original color.
        model.material.color = colorOrig;
    }
}
