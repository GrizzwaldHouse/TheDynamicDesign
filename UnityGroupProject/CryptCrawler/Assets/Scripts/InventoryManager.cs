using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuAct;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuAct)
        {
            InventoryMenu.SetActive(false);
            menuAct = false;
        }

        else if (Input.GetButtonDown("Inventory") && !menuAct)
        {
            InventoryMenu.SetActive(true);
            menuAct = true;
        }

    }
}