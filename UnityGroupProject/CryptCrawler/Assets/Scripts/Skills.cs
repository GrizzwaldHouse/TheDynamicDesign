using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class Skills : ScriptableObject
{
    [SerializeField] public int manacost; public float cooldownDuration = 5f; // Cooldown duration in seconds

    // This variable will be used to track the remaining cooldown time
    private float cooldownTimer = 0f;

    public virtual void Activate(PlayerController player)
    {
        // Check if the skill is off cooldown
        if (cooldownTimer <= 0f)
        {
            // Proceed with the skill activation logic
            ActivateSkill(player);
           
        }
        else
        {
            Debug.Log("Skill is on cooldown! Remaining time: " + cooldownTimer);
        }
    }
      public abstract void ActivateSkill(PlayerController player);
 }

   

