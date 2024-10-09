using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to the zombie game object and handles its behavior.
public class Zombie : MonoBehaviour
{
    // The initial health points of the zombie.
    [SerializeField] private int HP;

    // The animator component of the zombie game object.
    private Animator animator;

    // Called before the first frame update.
    private void Start()
    {
        // Get the animator component from the game object.
        animator = GetComponent<Animator>();
    }

    // Called when the zombie takes damage.
    public void TakeDamage(int damageAmount)
    {
        // Subtract the damage amount from the zombie's health points.
        HP -= damageAmount;

        // Check if the zombie's health points are less than or equal to 0.
        if (HP <= 0)
        {
            // Trigger the "DIE" animation.
            animator.SetTrigger("DIE");
        }
        else
        {
            // Trigger the "DAMAGE" animation.
            animator.SetTrigger("DAMAGE");
        }
    }
}