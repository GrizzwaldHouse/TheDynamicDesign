using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "New icewall")]
public class Icewall : Skills
{
    [SerializeField] GameObject IcewallObject;
    [SerializeField] int speed;
    // Start is called before the first frame update
    public override void ActivateSkill(PlayerController player)
    {
        // Instantiate the tornado at the player's position
        player.mana -= manacost;
        GameObject Icewall = Instantiate( IcewallObject, player.shootPos.position, player.shootPos.rotation);
        Rigidbody rb = Icewall.GetComponent<Rigidbody>();
        rb.velocity = Icewall.transform.forward * speed;

    }
}
