using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;

    [SerializeField] private int quantity;

    [SerializeField] private Sprite sprite;

    private InventoryManager intentoryManager;
    // Start is called before the first frame update
    void Start()
    {
        intentoryManager = GameObject.Find("IntentoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            intentoryManager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }
}
