using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New bolt")]
public class Lightningbolt : Skills
{
    // Start is called before the first frame update

    [SerializeField] GameObject lightningbolt;
    [SerializeField] public float heightaboveEnemy;
    [SerializeField] public float lightningspeed;

    public override void ActivateSkill(PlayerController player)
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Skeleton"))
            {
                player.mana -= manacost;
                Vector3 lightning = hit.transform.position + Vector3.up * heightaboveEnemy;
                GameObject skill = Instantiate(lightningbolt, lightning, Quaternion.identity);
                Rigidbody rb = skill.GetComponent<Rigidbody>();
                Vector3 direction = Vector3.down;
                rb.velocity = direction * lightningspeed;
            }
        }
    }

}
