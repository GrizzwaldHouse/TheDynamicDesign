using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
  // [SerializeField] private string itemName;
  
  [SerializeField] private int quantity;
 /// </summary>



   private InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Start()
    {

        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
       

            if (inventoryManager != null)
            {


                inventoryManager.AddItem(itemData, quantity);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("InventoryManager in not assigned or not found.");
            }


        }
}
