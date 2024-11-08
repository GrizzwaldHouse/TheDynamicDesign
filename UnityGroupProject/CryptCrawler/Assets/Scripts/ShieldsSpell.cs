using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    public LayerMask enemyLayer; // Layer for enemies

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if ((enemyLayer & (1 << other.gameObject.layer)) != 0)
        {
            // Prevent the enemy from passing through by pushing it back
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 directionAwayFromShield = other.transform.position - transform.position;
                enemyRigidbody.AddForce(directionAwayFromShield.normalized * 5f, ForceMode.Impulse);
            }
        }
    }
}

