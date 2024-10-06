using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;

    [SerializeField] int HP;
    Color colorOrig;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashColor());

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
