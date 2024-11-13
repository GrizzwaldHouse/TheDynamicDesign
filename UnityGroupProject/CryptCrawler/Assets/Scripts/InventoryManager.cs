using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] GameObject InventoryMenu;
    private bool menuActive;
    [SerializeField ] ItemSlotM[] itemSlots;
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    private List<ItemData> itemList = new List<ItemData>();
    private void Awake()
    {
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject); // Optional: Keep it across scenes
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") )
        {
            ToggleInventoryMenu();
        }


    }

    private void ToggleInventoryMenu()
    {
        menuActive = !menuActive;
        Time.timeScale = menuActive ? 1 : 0;
        InventoryMenu.SetActive(menuActive);
        UpdateInventoryUI();
    }
    public void AddItem(ItemData itemData, int quantity)
    {
        if (itemData == null)
        {
            Debug.LogWarning("ItemData is null. Cannot add item.");
            return; // Early exit if itemData is null
        }

        // Check if the item is already in the inventory
        if (inventory.ContainsKey(itemData))
        {
            if (itemData.isStackable)
            {


                UpdateExistingItem(itemData, quantity);
            }
            else
            {
                Debug.LogWarning($"Cannot add {quantity} {itemData.menuName} to the inventory. It's not stackable.");
            }
        }
        else
        {
            inventory[itemData] = quantity; // Add new item to inventory
        }

        UpdateInventoryUI(); // Call to update the UI
    }


    private void UpdateExistingItem(ItemData itemData, int quantity)
    {
        if (itemData.isStackable)
        {
            inventory[itemData] = Mathf.Min(inventory[itemData] + quantity, itemData.maxStackSize);
        }
        else
        {
            Debug.LogWarning($"Item not stackable: {itemData.menuName}");
        }
    }
    public void RemoveItem(ItemData itemData, int quantity)
    {
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] -= quantity;

            if (inventory[itemData] <= 0)
            {
                inventory.Remove(itemData);
            }

            UpdateInventoryUI(); // Update UI after removal
        }
        else
        {
            Debug.LogWarning($"Item {itemData.menuName} not found in inventory.");
        }
    }


    private void UpdateInventoryUI()
    {
        itemList = inventory.Keys.ToList();
        Debug.Log($"Updating Inventory UI. Item count: {itemList.Count}");


        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                var item = itemList[i];
                itemSlots[i].AddItem(item, inventory[item]);
                Debug.Log($"Updated slot {i} with item: {item.menuName}, Quantity: {inventory[item]}");

            }
            else
            {
                itemSlots[i].ClearSlot(); // Clear slot if no item
            }
        }
    }
}
