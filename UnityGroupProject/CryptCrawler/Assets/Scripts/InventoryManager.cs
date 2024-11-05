using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuAct;
    public ItemSlotM[] itemSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuAct)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuAct = false;
        }

        else if (Input.GetButtonDown("Inventory") && !menuAct)
        {
            Time .timeScale = 0;
            InventoryMenu.SetActive(true);
            menuAct = true;
        }

    }


    public void AddItem(string itemName, int quantity, Sprite itemSPrite)
    {
        //Debug.Log("itemName = " + itemName + "quantity = " + quantity + "itemSprite = " + itemSPrite);
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSPrite);
                return;
            }
        }

    }
}
