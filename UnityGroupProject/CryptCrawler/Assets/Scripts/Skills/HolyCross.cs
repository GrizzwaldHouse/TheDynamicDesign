using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New Cross")]
public class HolyCross : Skills
{
    [SerializeField] GameObject Cross;
    [SerializeField] public float heightaboveEnemy;
    [SerializeField] public float crosspeed;

    public override void ActivateSkill(PlayerController player)
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Skeleton"))
            {
                player.mana -= manacost;
                Vector3 Crossposition = hit.transform.position + Vector3.up * heightaboveEnemy;
                GameObject skill = Instantiate(Cross, Crossposition, Quaternion.identity);
                Rigidbody rb = skill.GetComponent<Rigidbody>();
                Vector3 direction = Vector3.up * heightaboveEnemy;
                rb.velocity = direction * crosspeed;
            }
        }


    }
}
