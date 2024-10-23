using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDam : MonoBehaviour
{
    // Damage dealt to the target per tick (interval)
    public float damagePerTick = 5f;
    // Time interval in seconds between each damage tick
    public float tickRate = 1f;

    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the IDamage component from the object that entered the lava pit
        IDamage target = other.GetComponent<IDamage>();

        // Check if the object has an IDamage component (indicating it can take damage)
        if (target != null)
        {
            // Log a message indicating that a target has entered the lava pit
            Debug.Log("Target entered lava pit: " + other.gameObject.name);
            // Start the coroutine to apply damage over time to the target
            StartCoroutine(ApplyLavaDamage(target));
        }
        else
        {
            // Log a message indicating that the object does not have an IDamage component
            Debug.Log("No IDamage component found on: " + other.gameObject.name);
        }
    }

    // This method is called when an object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Log a message indicating that the target has exited the lava pit
        Debug.Log("Target exited lava pit: " + other.gameObject.name);
        // Here you can implement logic to stop damage if necessary, though it's currently not implemented.
    }

    // Coroutine that applies continuous damage to the target while it is inside the lava pit
    private IEnumerator ApplyLavaDamage(IDamage target)
    {
        // Loop indefinitely to apply damage continuously
        while (true)
        {
            // Apply damage to the target
            target.takeDamage((int)damagePerTick);
            // Log the amount of damage being applied
            Debug.Log("Applying lava damage: " + damagePerTick);
            // Wait for the specified tickRate before applying damage again
            yield return new WaitForSeconds(tickRate);
        }
    }
}
