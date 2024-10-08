using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int HP;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            animator.SetTrigger("DIE");

        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }

    }
}