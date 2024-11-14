using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SecretBoss : MonoBehaviour, IDamage
{
    private enum AttackType{
        Regularattack,
        Aoeattack
    }
    // Start is called before the first frame update
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] int viewAngle;

    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int ExpWorth;

    [SerializeField] public int HP;
    [SerializeField] int rotateSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] Image enemyHPbar;
    [SerializeField] int aoeRadius;
    [SerializeField] int aoeDamage;
    [SerializeField] GameObject aoeEffect;


    bool isShooting;
    bool SecondPhase;
    bool playerInRange;
    bool isRoaming;
    public bool isDead;
    private int HPorig;
    bool isHit;
    private AttackType chosenattackType;
    float angleToplayer;
    float stoppingDistOrig;

    Vector3 playerDir;

    Vector3 startingPos;

    Color colorOg;

    Coroutine someCo;

    // Start is called before the first frame update
    void Start()
    {
        colorOg = model.material.color;
        gamemanager.instance.UpdateGameGoal(1);
        HPorig = HP;

        UpdateEnemyUI();
        
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        SecondPhase = false;
        isDead = false;
        chosenattackType = AttackType.Regularattack;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (isShooting == false)
        {
            anim.SetFloat("speed", agent.velocity.normalized.magnitude);
        }
        if(playerInRange && canSeePlayer())
        {

        } 
        CheckSecondPhase();
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

                if (isShooting == false && angleToplayer < shootAngle)
                {
                    // Allow shooting in both phases
                    if (SecondPhase)
                    {
                        chosenattackType = (Random.Range(0, 2) == 0) ? AttackType.Regularattack : AttackType.Aoeattack;
                    }
                    else
                    {
                        chosenattackType = AttackType.Regularattack; // Always regular attack in the first phase
                    }

                    if (chosenattackType == AttackType.Regularattack)
                    {
                        StartCoroutine(Shoot());
                    }
                    else if (chosenattackType == AttackType.Aoeattack)
                    {
                        AoeAtack();
                    }



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

    IEnumerator Shoot()
    {
        if (!isHit)
        {
            isShooting = true;
            anim.SetTrigger("attack");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            Instantiate(bullet, shootPos.position, transform.rotation);

            isShooting = false;
        }
    }
    public void takeDamage(int amount)
    {
        isHit = true;
        HP -= amount;
        UpdateEnemyUI();

        if (someCo != null)
        {
            StopCoroutine(someCo);
            isRoaming = false;
        }
        anim.SetTrigger("hit");
        agent.SetDestination(gamemanager.instance.player.transform.position);

        StartCoroutine(flashColor());

        if (HP <= 0)
        {
            gamemanager.instance.UpdateGameGoal(-1);
            isDead = true;
            gamemanager.instance.accessPlayer.gainExperience(ExpWorth);
            Destroy(gameObject);
        }
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

    }
    void CheckSecondPhase()
    {
        Debug.Log($"Current HP: {HP}, Original HP: {HPorig}, Second Phase: {SecondPhase}");
        if (!SecondPhase && HP <= HPorig / 2)
        {
            // Transition to the second phase
            SecondPhase = true;
            Debug.Log("Boss has entered the second phase!");
            agent.speed *= 2;
        }
    }
     void AoeAtack()
     {
        
        Debug.Log("Performing AoE attack!");

        // Instantiate the AoE particle effect at the boss's position
        if (aoeEffect != null)
        {
            Instantiate(aoeEffect,transform.position, Quaternion.identity) ;
        }

       new WaitForSeconds(1f);

        // Find all colliders within the AoE radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider belongs to a player
            if (hitCollider.CompareTag("Player"))
            {
                // Apply damage to the player
                hitCollider.GetComponent<PlayerController>().takeDamage(aoeDamage);
            }
        }
    }
    void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Draw a wire sphere at the boss's position with the AoE radius
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}

