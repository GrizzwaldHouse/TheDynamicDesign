using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Icewallogic : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float freezeDuration; // Duration for which the enemy will be frozen

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a NavMeshAgent and is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Skeleton"))
        {
            IDamage damageable = collision.gameObject.GetComponent<IDamage>();

            NavMeshAgent navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null && damageable != null)
            {
                damageable.takeDamage(damageAmount);
                StartCoroutine(FreezeEnemy(navMeshAgent));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator FreezeEnemy(NavMeshAgent navMeshAgent)
    {
        // Disable the NavMeshAgent to stop movement
        navMeshAgent.isStopped = true; // Stop the agent's movement
        navMeshAgent.enabled = false; // Disable the agent

        // Optionally, you can add a visual effect or change the enemy's appearance here

        // Wait for the freeze duration
        yield return new WaitForSeconds(freezeDuration);

        // Re-enable the NavMeshAgent
        navMeshAgent.enabled = true; // Re-enable the agent
        navMeshAgent.isStopped = false; // Allow the agent to move again

        // Optionally, you can remove the visual effect or change the enemy's appearance back here
    }

}

