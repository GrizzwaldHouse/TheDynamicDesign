using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Mana : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] enum magictype { Fire, Ice, Holy, Lightning, Wind }
    [SerializeField] magictype type;
    [SerializeField] Rigidbody rb;
    [SerializeField] float freezeDuration;
    [SerializeField] float firetickrate;
    [SerializeField] int damagePerTick;
    [SerializeField] float tickDuration;
    [SerializeField] public int damageamount;
    [SerializeField] public int Manacost;
    [SerializeField] int speed;
    [SerializeField] int destroytime;
    [SerializeField] float damageRange;
    [SerializeField] int maxChainJumps;
    [SerializeField] float chainJumpRange;
    [SerializeField] int knockbackForce;
    bool isBurning;
    bool isFrozen;

    // Start is called before the first frame update
    void Start()
    {
        if (type == magictype.Fire)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }
        else if (type == magictype.Ice)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }
        else if (type == magictype.Holy)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }
        else if (type == magictype.Wind)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }
        else if (type == magictype.Lightning) 
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }

    }

    private IEnumerator ApplyBurnEffect(IDamage target)
    {
        if (isBurning||target ==null) yield break; // Exit if already burning or target is null
        isBurning = true;

        Debug.Log("Burn effect started.");
        float elapsed = 0f;
        float tickTime = 0f;

        while (elapsed < tickDuration)
        {
            tickTime += Time.deltaTime;
            if (tickTime >= firetickrate)
            {
                if (target != null) // Check if target is still valid
                {
                    target.takeDamage(damagePerTick);
                    Debug.Log("Applying burn damage: " + damagePerTick);
                }
                tickTime = 0f;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        isBurning = false; // Reset for future hits
    }


    private void ApplyFreezeEffect(NavMeshAgent target)
    {
        if (isFrozen|| target==null || !target.isActiveAndEnabled) return; // Exit if already frozen
        isFrozen = true;
        // Stop the NavMesh agent
        target.isStopped = true; // Set the agent's isStopped flag to true

        // Start the freeze coroutine
        StartCoroutine(UnfreezeTarget(target));
    }

    private IEnumerator UnfreezeTarget(NavMeshAgent target)
    {
        float elapsed = 0f;


        while (elapsed < freezeDuration)
        {
            if(target ==null || !target.isActiveAndEnabled) yield break;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isFrozen = false; // Reset the frozen state
        if (target != null && target.isActiveAndEnabled)
        {
            target.isStopped = false; // Set the agent's isStopped flag to false
        }
        // Resume the NavMesh agent
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Skeleton"))
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (dmg != null)
            {
                Debug.Log("Damage applied: " + damageamount);
                
                if(collision.gameObject.CompareTag("Skeleton") && type == magictype.Holy)
                {
                    damageamount *= 2;
                }
                dmg.takeDamage(damageamount);
                if (type == magictype.Fire)
                {
                    StartCoroutine(ApplyBurnEffect(dmg));
                }
                else if (type == magictype.Ice)
                {
                    if (agent != null)
                    {
                        ApplyFreezeEffect(agent);
                    }
                    else
                    {
                        Debug.Log("No NavMeshAgent component found on the enemy.");
                    }
                }
                else if (type == magictype.Wind)
                {
                    
                    Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

                    // Apply knockback force to the enemy's Rigidbody
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                    Debug.Log("Applying wind knockback.");

                }
                else if (type == magictype.Lightning)
                {
                    StartCoroutine(ChainLightning(collision.transform, damageamount));
                    
                }
                    Destroy(gameObject);
                
            }
            else
            {
                Debug.Log("No IDamage component found on the enemy.");
            }
        }
       
    }
    private IEnumerator ChainLightning(Transform initialTarget, int damageAmount)
    {
        Transform currentTarget = initialTarget;
        int jumps = 0;

        // Create a list to keep track of already hit targets to avoid repeated hits
        HashSet<Transform> hitTargets = new HashSet<Transform>();
        hitTargets.Add(currentTarget); // Add the initial target to the hit list

        while (jumps < maxChainJumps)
        {
            // Find nearby enemies within the chain jump range
            Collider[] hitColliders = Physics.OverlapSphere(currentTarget.position, chainJumpRange);
            Transform nextTarget = null;

            foreach (var hitCollider in hitColliders)
            {
                // Ensure we only consider enemies that have not been hit yet
                if (hitCollider.CompareTag("Enemy") && hitCollider.transform != currentTarget && !hitTargets.Contains(hitCollider.transform))
                {
                    nextTarget = hitCollider.transform;
                    break; // Take the first found enemy within range
                }
            }

            if (nextTarget == null) break; // No more targets to jump to

            // Apply damage to the next target
            IDamage nextDamage = nextTarget.GetComponent<IDamage>();
            if (nextDamage != null)
            {
                nextDamage.takeDamage(damageAmount);
                Debug.Log("Chain lightning damage applied: " + damageAmount);

                currentTarget = nextTarget; // Move to the next target
                jumps++;
                hitTargets.Add(currentTarget); // Add to the list of hit targets
            }
            else
            {
                break; // No IDamage component found on the next target
            }

            yield return new WaitForSeconds(0.01f); // Wait for the next frame
        }
        Destroy(gameObject);
        yield return null;
    }


}
    


