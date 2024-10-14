using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [SerializeField] int HP;
    [SerializeField] int experience;
    [SerializeField] int level;
    [SerializeField] int experienceToNextLevel;
  [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDam;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPorig;

    bool isSprinting;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        movement();
        Sprint();
    }

    void movement()
    {
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += moveDir * speed * Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButton("Shoot") && !gamemanager.instance.isPaused && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreMask))
        {
            //Debug.Log(hit.collider.name);

            IDamage damage = hit.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.takeDamage(shootDam);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    IEnumerator DamageFlash()
    {
        gamemanager.instance.PlayerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.PlayerDamageScreen.SetActive(false);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdatePlayerUI();
        StartCoroutine(DamageFlash());

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    public void gainHealth(int amount)
    {
        HP = Mathf.Min(HP + amount, HPorig);
        UpdatePlayerUI();
    }
    public void gainExperience(int amount)
    {
        if(experience>= experienceToNextLevel)
        {
            levelUp();
        }
    }
    public int GetLevel() { return level; }
    public int GetHealth() { return HP; }
    void levelUp()
    {
        level++;
        experience = calculateExperienceToNextLevel();
        experienceToNextLevel = calculateExperienceToNextLevel();
        HP = HPorig; //reset health to max
        UpdatePlayerUI();
        Debug.Log("Level Up! You are now level" + level);
    }
  private int calculateExperienceToNextLevel()
    {
        return (int)Mathf.Pow(level, 2) * 100;
    }
   

    IEnumerator DamageFlash()
    {
        gamemanager.instance.PlayerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.PlayerDamageScreen.SetActive(false);
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadSystem()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        level = data.level;
        HP = data.health;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
    public void UpdatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPorig;
    }
}