using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]public float damageRadius;
    [SerializeField] public int damageAmount;
    [SerializeField] public float lifetime;
    


    private void Start()
    {
        // Destroy the fireball after its lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the fireball has collided with the ground
 
            // Create an overlap sphere for damage detection
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
            foreach (var hitCollider in hitColliders)
            {
                // Check if the hit collider has a damageable component
                IDamage damageable = hitCollider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.takeDamage(damageAmount); // Apply damage
                }
            }

            // Optionally, destroy the fireball after applying damage
            Destroy(gameObject);
    }
}
