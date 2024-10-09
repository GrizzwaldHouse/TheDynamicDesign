using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpellShooter : MonoBehaviour
{
    // The magic spell scriptable object
    public MagicSpell magicSpell;
    public Transform castPoint;

    // The player's current magic spell cooldown timer
    float magicSpellCooldownTimer = 0f;

    // Flag to check if the player is currently shooting
    bool isShooting = false;

    void Update()
    {
        // Check if the player is not currently shooting and the cooldown timer has expired
        if (!isShooting && magicSpellCooldownTimer <= 0f)
        {
            // Check if the player presses the magic spell button
            if (Input.GetButtonDown("MagicSpell"))
            {
                // Shoot a magic spell
                ShootMagicSpell();
            }
        }

        // Update the cooldown timer
        magicSpellCooldownTimer -= Time.deltaTime;
    }

    void ShootMagicSpell()
    {
        // Instantiate a new magic spell projectile
        GameObject magicSpellObject = Instantiate(magicSpell.magicSpellPrefab, castPoint.position, castPoint.rotation);

        // Set the magic spell's velocity
        magicSpellObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * magicSpell.magicSpellSpeed;

        // Add a script to handle the magic spell's damage
        MagicSpellProjectile projectile = magicSpellObject.AddComponent<MagicSpellProjectile>();
        projectile.damage = magicSpell.magicSpellDamage;

        // Set the cooldown timer
        magicSpellCooldownTimer = magicSpell.magicSpellCooldown;

        // Set the isShooting flag to true
        isShooting = true;
    }
}

public class MagicSpellProjectile : MonoBehaviour
{
    // The damage dealt by the magic spell
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // Get the IDamage component from the collided object
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();

        // If the collided object has an IDamage component, call its takeDamage method
        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }

        // Destroy the magic spell projectile
        Destroy(gameObject);
    }
}

