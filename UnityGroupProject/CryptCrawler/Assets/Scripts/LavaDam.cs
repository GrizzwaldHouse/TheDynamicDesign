using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDam : MonoBehaviour
{
    // Damage dealt to the target per tick (interval)
    public float damagePerTick = 5f;
    // Time interval in seconds between each damage tick
    public float tickRate = 1f;
    private IDamage currentTarget;
    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the IDamage component from the object that entered the lava pit
        currentTarget = other.GetComponent<IDamage>();

        // Check if the object has an IDamage component (indicating it can take damage)
        if (currentTarget != null)
        {
            // Log a message indicating that a target has entered the lava pit
            Debug.Log("Target entered lava pit: " + other.gameObject.name);
            // Start the coroutine to apply damage over time to the target
            InvokeRepeating(nameof(ApplyLavaDamage),0f,tickRate);
        }
    }

    // This method is called when an object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (currentTarget != null && other.GetComponent<IDamage>() == currentTarget)
        {
            currentTarget = null;
            CancelInvoke(nameof(ApplyLavaDamage));
        }
    }

    // Method that applies damage to the target
    private void ApplyLavaDamage()
    {
        if (currentTarget != null)
        {
            // Apply damage to the target
            currentTarget.takeDamage((int)damagePerTick);
            // Log the amount of damage being applied
            Debug.Log("Applying lava damage: " + damagePerTick);
        }
    }
}
