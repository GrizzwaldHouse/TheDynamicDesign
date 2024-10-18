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
    [SerializeField] Animator anim;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] Transform headPos;
    [SerializeField] int shootAngle;

    float angleToplayer;
    bool playerInRange;
    public bool isDead;
    bool isSwinging;
    public Image enemyHPbar;
    float playerHealth;
    bool isRoaming;
    float stoppingDistOrig;
    bool isHit;

    float angleToPlayer;
    Vector3 playerDir;
    Color colorOg;
    int HPorig;
    Coroutine someCo;
    Vector3 startingPos;

    void Start()
    {
        HPorig = HP;
        colorOg = model.material.color;
        enemyHPbar.fillAmount = 1f;
        UpdateEnemyUI();
        gamemanager.instance.UpdateGameGoal(1);
        playerHealth = gamemanager.instance.playerHPBar.fillAmount;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    void Update()
    {
        if (isSwinging == false)
        {
            anim.SetFloat("speed", agent.velocity.normalized.magnitude);
        }
        if (playerInRange && !canSeePlayer())
        {
            if (!isRoaming && agent.remainingDistance < 0.05f)
            {
                someCo = StartCoroutine(Roam());
            }
        }
        else if (!playerInRange)
        {
            if (!isRoaming && agent.remainingDistance < 0.05f)
            {
                someCo = StartCoroutine(Roam());
            }
        }
    }
    IEnumerator Roam()
    {
        isRoaming = true;
        yield return new WaitForSeconds(roamPauseTime);

        //can move this code up
        agent.stoppingDistance = 0;
        Vector3 randomDist = Random.insideUnitSphere * roamDist;
        randomDist += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDist, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
        someCo = null;
    }
    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        angleToplayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToplayer <= viewAngle)
            {
                agent.SetDestination(gamemanager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (isSwinging == false && angleToplayer < shootAngle)
                {
                    StartCoroutine(Attack());
                }

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }

        agent.stoppingDistance = 0;

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
            agent.stoppingDistance = 0;
        }
    }

    IEnumerator Attack()
    {
        // Prevent multiple attacks at once
        if (!isHit && transform.position.x != stoppingDistOrig)
        {
            isSwinging = true; // Prevent further attacks until the coroutine is finished
            // Call the sword collider method to deal damage to the player
            anim.SetTrigger("attack");
            // Check for player collision with sword collider
            Collider[] hitColliders = Physics.OverlapSphere(swordTransform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    gamemanager.instance.accessPlayer.takeDamage(damageAmount);
                }
            }
            yield return new WaitForSeconds(attackRate);
            isSwinging = false; // Reset for the next attack
        }
    }

    public void takeDamage(int amount)
    {
        isHit = true;
        anim.SetTrigger("damage");
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
        StartCoroutine(flashColor());
        isHit = false;
    }
    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOg;
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
