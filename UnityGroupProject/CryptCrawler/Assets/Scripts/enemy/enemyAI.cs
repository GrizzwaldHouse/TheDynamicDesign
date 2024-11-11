using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] int viewAngle;

    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] Image enemyHPbar;
    [SerializeField] int ExpWorth;

  
    //  private ObjectSpawner spawner; // Reference to the spawner
    bool isShooting;
    bool playerInRange;
    bool isRoaming;
    public bool isDead;
    private bool isBeingDestoryed;
    int HPorig;
    bool isHit;
    private ObjectSpawner objectSpawner;
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
        UpdateEnemyUI();
        HPorig = HP;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        objectSpawner = FindObjectOfType<ObjectSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting == false)
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

                if (isShooting == false && angleToplayer < shootAngle)
                {
                    StartCoroutine(Shoot());
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
            Instantiate(bullet, shootPos.position, transform.rotation);
            yield return new WaitForSeconds(shootRate);

            isShooting = false;
        }
    }
    public void takeDamage(int amount)
    {
        if(isBeingDestoryed) return; 
        isHit = true;
        HP -= amount;
        DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.yellow);
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
            gamemanager.instance.accessPlayer.gainExperience(ExpWorth);
            OnDestroy();
            isBeingDestoryed = true;
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
    private void OnDestroy()
    { 
        // Prevent further actions if already being destroyed
        if (isBeingDestoryed) return;
        isBeingDestoryed = true; // Set flag to indicate destruction in progress
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
        }
       
                                  // Call ObjectDestroyed when this enemy is destroyed
        if (objectSpawner != null)
        {
            objectSpawner.ObjectDestroyed(gameObject);
        } // Notify the spawner that an object has been destroyed
     
    }
    public void gainHealth(int amount)
    {

    }
}