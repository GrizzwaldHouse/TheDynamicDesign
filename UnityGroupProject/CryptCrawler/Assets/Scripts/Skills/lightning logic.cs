using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightninglogic : MonoBehaviour
{
    [SerializeField] int damageamount;

    private void OnCollisionEnter(Collision collision)
    {
        IDamage damageable = collision.gameObject.GetComponent<IDamage>();

        damageable.takeDamage(damageamount);
        
        
        Destroy(gameObject);
    } 
   
}
