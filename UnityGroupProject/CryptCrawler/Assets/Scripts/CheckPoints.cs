using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] Renderer model;

    Color colorOrig;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(flashColor());
        }
    }

    IEnumerator flashColor()
    {
        
        model.material.color = Color.red;

        
        yield return new WaitForSeconds(0.5f);

        
        model.material.color = colorOrig;
    }


}
