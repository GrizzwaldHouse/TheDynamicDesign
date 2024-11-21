using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDam : MonoBehaviour
{
    // Damage dealt to the target per tick (interval)
    public float damagePerTick;
    // Time interval in seconds between each damage tick
    public float tickRate ;
    private IDamage currentTarget;
    private Coroutine damageCoroutine;
    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the IDamage component from the object that entered the lava pit
        currentTarget = other.GetComponent<IDamage>();

        // Check if the object has an IDamage component (indicating it can take damage)
        if (currentTarget != null)
        {

            // Start the coroutine to apply damage over time to the target
            damageCoroutine = StartCoroutine(ApplyLavaDamage());
        }
    }

    // This method is called when an object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (currentTarget != null && other.GetComponent<IDamage>() == currentTarget)
        {
            currentTarget = null;
           if (damageCoroutine != null)
            {
                damageCoroutine = null; 
            }
        }
    }

    // Method that applies damage to the target
    private IEnumerator ApplyLavaDamage()
    {
        while (currentTarget != null)
        {
            // Apply damage to the target
            currentTarget.takeDamage((int)damagePerTick);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
