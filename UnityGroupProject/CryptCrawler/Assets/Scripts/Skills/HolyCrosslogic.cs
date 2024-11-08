using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolyCrosslogic : MonoBehaviour
{
    [SerializeField] int damageamount;
    private void OnCollisionEnter(Collision collision)
    {
        IDamage damageable = collision.gameObject.GetComponent<IDamage>();
        if(collision.gameObject.CompareTag("Skeleton")){
            damageamount *= 2;
        }
        damageable.takeDamage(damageamount);
        Destroy(gameObject);
    }
}
