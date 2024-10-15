using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] enum magictype { Fire, Ice, lighting }
    [SerializeField] magictype type;
    [SerializeField] Rigidbody rb;
    [SerializeField] float firetickrate;
    [SerializeField] int damagePerTick;
    [SerializeField] float tickDuration;
    [SerializeField]  public int damageamount;
    [SerializeField] public int Manacost;
    [SerializeField] int speed;
    [SerializeField] int destroytime;
    [SerializeField] float damageRange;

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
        else if (type == magictype.lighting)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroytime);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(damageamount);
            }
            if (type == magictype.Ice || type == magictype.lighting)
            {
                Destroy(gameObject);
            }
            else if (type == magictype.Fire)
            {
                StartCoroutine(ApplyFireDamageOverTime(dmg));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ApplyFireDamageOverTime(IDamage target)
    {
        float elapsed = 0f; // Time elapsed since the DOT started

        // Continue applying damage at intervals while within the DOT duration
        while (elapsed < tickDuration)
        {
            // Apply damage per tick
            target.takeDamage(damagePerTick);

            // Wait for the next tick
            yield return new WaitForSeconds(firetickrate);

            // Update elapsed time
            elapsed += firetickrate;
        }
    }
}
