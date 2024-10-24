using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] enum damagetype { bullet, stationary, chaser }
    [SerializeField] damagetype type;
    [SerializeField] Rigidbody rb;
    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroytime;
    [SerializeField] float damageInterval;
    [SerializeField] float damageRange;

    // Start is called before the first frame update
    void Start()
    {
        if (type == damagetype.bullet)
        {
            rb.velocity = (gamemanager.instance.player.transform.position - transform.position).normalized *speed;
            Destroy(gameObject, destroytime);
        }
        else if (type == damagetype.chaser)
        {
            Destroy(gameObject, destroytime);
        }
        else if (type == damagetype.stationary)
        {
            StartCoroutine(DealDamageOverTime());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }
        if (type == damagetype.bullet || type == damagetype.chaser)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DealDamageOverTime()
    {
        while (true) // Infinite loop; will break when the object is destroyed
        {
            // Find all colliders within the damage range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.isTrigger)
                {
                    continue;
                }

                IDamage dmg = hitCollider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(damageAmount);
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damagetype.chaser)
        {
            rb.velocity = (gamemanager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
