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
        intentoryManager = GameObject.Find("InventoryCan").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            intentoryManager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }
}
