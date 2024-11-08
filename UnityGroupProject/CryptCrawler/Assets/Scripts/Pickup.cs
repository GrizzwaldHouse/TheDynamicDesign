using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
   // [SerializeField] Wands wand;
    [SerializeField] ItemData Potion;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // gamemanager.instance.accessPlayer.getWandstats(wand);
            gamemanager.instance.accessPlayer.getItemStats(Potion);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}

