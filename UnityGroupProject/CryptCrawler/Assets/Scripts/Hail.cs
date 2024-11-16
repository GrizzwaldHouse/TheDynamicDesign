using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hail : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage; // Damage dealt by the hailstone

    private void OnTriggerEnter(Collider other)
    {
        // Check for collision with any object
        if (other.gameObject.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            // Apply damage to the player
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(damage); // Implement this method in your PlayerHealth script
            }

            // Destroy the hailstone after hitting the player
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground")) // Optional: Destroy hailstone on ground hit
        {
            Destroy(gameObject);
        }

    }
}

