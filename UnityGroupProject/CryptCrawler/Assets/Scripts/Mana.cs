using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mana : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] enum magictype { Fire, Ice, Holy }
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


    }

    private IEnumerator ApplyBurnEffect(IDamage target)
    {
        if (isBurning) yield break; // Exit if already burning
        isBurning = true;

        Debug.Log("Burn effect started.");
        float elapsed = 0f;
        float tickTime = 0f;

        while (elapsed < tickDuration)
        {
            tickTime += Time.deltaTime;
            if (tickTime >= firetickrate)
            {
                target.takeDamage(damagePerTick);
                Debug.Log("Applying burn damage: " + damagePerTick);
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
        if (isFrozen) return; // Exit if already frozen
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
            elapsed += Time.deltaTime;
            yield return null;
        }

        isFrozen = false; // Reset the frozen state
        target.isStopped = false; // Set the agent's isStopped flag to false
       ; // Resume the NavMesh agent
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            if (dmg != null)
            {
                Debug.Log("Damage applied: " + damageamount);
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
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("No IDamage component found on the enemy.");
            }
        }
    }


}
