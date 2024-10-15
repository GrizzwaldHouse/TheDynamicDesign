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
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] int viewAngle;

    [SerializeField] int HP;
    [SerializeField] int rotateSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    public bool isDead;
    public Image enemyHPbar;

    float angleToplayer;

    Vector3 playerDir;

    Color colorOg;
    int HPorig;

    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        colorOg = model.material.color;
        enemyHPbar.fillAmount = 1f;
        UpdateEnemyUI();
        gamemanager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInRange && canSeePlayer())
        {
        }
    }

    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        angleToplayer = Vector3.Angle(playerDir, transform.forward);
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

                if (isShooting == false)
                {
                    StartCoroutine(Shoot());
                }
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

    IEnumerator Shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gamemanager.instance.player.transform.position);

        StartCoroutine(flashColor());
        UpdateEnemyUI();

        if (HP <= 0)
        {
            gamemanager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
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
}