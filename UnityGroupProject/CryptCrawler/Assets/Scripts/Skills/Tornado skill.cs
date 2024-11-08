using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New Tornado")]
public class Tornadoskill : Skills
{
    [SerializeField] GameObject TornadObject;
    [SerializeField] public float pullRadius;
    [SerializeField] public float pullForce;
    [SerializeField] public float Duration;
    [SerializeField] public int speed;

    public override void ActivateSkill(PlayerController player)
    {
        // Instantiate the tornado at the player's position
        player.mana -= manacost;
        GameObject tornado = Instantiate( TornadObject, player.shootPos.position, player.shootPos.rotation);
        TornadoLogic tornadoScript = tornado.GetComponent<TornadoLogic>();
        Rigidbody rb = tornado.GetComponent<Rigidbody>();
        rb.velocity = tornado.transform.forward * speed;
        tornadoScript.Initialize(pullRadius, pullForce, Duration);
    }
}
