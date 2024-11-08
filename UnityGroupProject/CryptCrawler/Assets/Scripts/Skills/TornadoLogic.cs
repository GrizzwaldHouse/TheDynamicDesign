using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float pullRadius;
    [SerializeField] public float pullForce;
    [SerializeField] public float Duration;

    // Update is called once per frame
    public void Initialize(float radius, float force, float duration)
    {
        this.pullRadius = radius;
        this.pullForce = force;
        this.Duration = duration;
    }

    private void FixedUpdate()
    {
        // Find all colliders within the pull radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider has a Rigidbody and is an enemy
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Skeleton"))
            {
                Rigidbody enemyRigidbody = hitCollider.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    // Calculate the direction to the tornado center
                    Vector3 direction = (transform.position - hitCollider.transform.position).normalized;
                    // Apply force to pull the enemy towards the tornado
                    enemyRigidbody.AddForce(direction * pullForce);
                }
            }
        }
    }
}
