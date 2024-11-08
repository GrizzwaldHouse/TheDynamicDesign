using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of projectiles that fly towards a target player.
public class FlyAtPlayer : MonoBehaviour
{

    [SerializeField] private float speed; // Speed at which the projectile moves towards the target
    [SerializeField] private float lifetime; // Time in seconds before the projectile self-destructs

    [SerializeField] Transform target; // The target the projectile will move towards (the player)
    [SerializeField] private int damage; // Damage dealt to the target upon collision
    [SerializeField] private GameObject hitEffect; // Particle effect to instantiate on hit
    private string targetTag;
    public int PrefabIndex { get; set; } // Property to store the index of the prefab for pooling purposes

    // Start is called before the first frame update
    private void Start()
    {
        // Start a coroutine to handle self-destruction after a certain time
        Invoke(nameof(SelfDestructAfterTime), lifetime);
        SetTargetTag(targetTag);
    }

    // Update is called once per frame
    private void Update()
    {
        // Move towards the target if it is set
        if (target != null)
        {
            MoveTowardsTarget(); // Call the method to move the projectile towards the target
        }
        else
        {
            SetTargetTag(targetTag);
        }
    }

    // Method to set the target tag for the projectile
    public void SetTargetTag(string tag)
    {
        targetTag = tag;

        // Find the target in the scene with the specified tag
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform; // Set the target to the found object's transform
        }
    }

    // Method to move the projectile towards the target
    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Move the projectile towards the target
            transform.position += direction * speed * Time.deltaTime;

          
            // Check if the projectile has reached the target
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject); // Destroy the projectile upon reaching the target
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collides with the target
        if (other.CompareTag(target.tag))
        {
            // Implement damage or effect logic here
            Debug.Log($"Hit target: {other.name}");

            // Return the projectile to the pool or destroy it
            TriggerProjectile triggerProjectile = FindObjectOfType<TriggerProjectile>();
            if (triggerProjectile != null)
            {
                triggerProjectile.ReturnProjectileToPool(gameObject); // Return to pool
            }
            else
            {
                Destroy(gameObject); // Destroy if no pool is available
            }
        }
    }
    // Coroutine to handle self-destruction after a certain time
    private void SelfDestructAfterTime()
    {
        // Deactivate the projectile instead of destroying it for pooling
        gameObject.SetActive(false); // Deactivate the projectile to make it reusable
    }
}