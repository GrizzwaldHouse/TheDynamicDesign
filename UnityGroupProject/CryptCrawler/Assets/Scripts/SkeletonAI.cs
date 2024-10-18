using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkeletonAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform swordTransform; // Transform for sword collider
    [SerializeField] int viewAngle;
    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;
    [SerializeField] int damageAmount; // Damage dealt to the player
    [SerializeField] float attackRange; // Range for melee attack
    [SerializeField] float attackRate; // Time between attacks
    [SerializeField] int ExpWorth;

    bool playerInRange;
    public bool isDead;
    public Image enemyHPbar;
    float playerHealth;

    float angleToPlayer;
    Vector3 playerDir;
    Color colorOg;
    int HPorig;

    void Start()
    {
        HPorig = HP;
        colorOg = model.material.color;
        enemyHPbar.fillAmount = 1f;
        UpdateEnemyUI();
        gamemanager.instance.UpdateGameGoal(1);
        playerHealth = gamemanager.instance.playerHPBar.fillAmount;
    }

    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            // If the player is in range and can be seen, attack
            if (Vector3.Distance(transform.position, gamemanager.instance.player.transform.position) <= attackRange)
            {
                StartCoroutine(Attack());
            }
        }
    }

    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(gamemanager.instance.player.transform.position);
                faceTarget();
                return true;
            }
        }
        return false;
    }

    void faceTarget()
    {
        Quaternion rotate = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * rotateSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator Attack()
    {
        // Prevent multiple attacks at once
        if (!isDead)
        {
            isDead = true; // Prevent further attacks until the coroutine is finished
            // Call the sword collider method to deal damage to the player
            DealDamageToPlayer();
            yield return new WaitForSeconds(attackRate);
            isDead = false; // Reset for the next attack
        }
    }

    void DealDamageToPlayer()
    {
        
        // Check for player collision with sword collider
        Collider[] hitColliders = Physics.OverlapSphere(swordTransform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                if (playerHealth != 0)
                {
                    playerHealth -= damageAmount;
                }
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdateEnemyUI();

        if (HP <= 0)
        {
            gamemanager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
            if (gamemanager.instance.accessPlayer != null)
            {
                gamemanager.instance.accessPlayer.gainExperience(ExpWorth);
                Debug.Log("Enemy died, gaining " + ExpWorth + " XP!");
            }
            else
            {
                Debug.LogError("accessPlayer is null!");
            }
        }
    }

    public void UpdateEnemyUI()
    {
        enemyHPbar.fillAmount = (float)HP / HPorig;
    }

    public void gainHealth(int amount)
    {
        throw new System.NotImplementedException();
    }
}
