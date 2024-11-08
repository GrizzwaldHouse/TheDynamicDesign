using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New meteor")]
public class Meteorskill : Skills
{
    [SerializeField] GameObject Meteor;
    [SerializeField] public float heightaboveEnemy;
    [SerializeField] public float meteorspeed;

    public override void ActivateSkill(PlayerController player)
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Skeleton"))
            {
                player.mana -= manacost;
                Vector3 Meteorposition = hit.transform.position + Vector3.up * heightaboveEnemy;
                GameObject skill = Instantiate(Meteor, Meteorposition, Quaternion.identity);
                Rigidbody rb = skill.GetComponent<Rigidbody>();
                Vector3 direction = Vector3.down;
                rb.velocity = direction * meteorspeed;
            }
        }
       
        
    }
}
