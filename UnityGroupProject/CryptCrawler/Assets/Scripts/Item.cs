using System.Collections; // Importing the System.Collections namespace for collection-related functionality
using System.Collections.Generic; // Importing the System.Collections.Generic namespace for generic collections
using UnityEngine; // Importing the UnityEngine namespace to access Unity-specific classes and functions

public class Item : MonoBehaviour // Item class inherits from MonoBehaviour, allowing it to be attached to GameObjects
{
    // Serialized field to expose the ItemData ScriptableObject in the Unity Inspector
    [SerializeField] private ItemData itemData;

    // Serialized field for the quantity of this item; currently commented out
    // [SerializeField] private string itemName;

    // Serialized field to store the quantity of this item in the inventory
    [SerializeField] private int quantity;

    // Reference to the InventoryManager instance, used to manage the inventory
    private InventoryManager inventoryManager;
    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found in the scene.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Find and assign the InventoryManager instance in the scene
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Method called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the inventoryManager has been successfully assigned
        if (inventoryManager != null)
        {
            // Add the item to the inventory using the itemData and quantity
            inventoryManager.AddItem(itemData, quantity);
            // Check if the item is a currency item
            if (itemData.category == ItemCategory.Currency)
            {
                CoinSystem coinSystem = FindObjectOfType<CoinSystem>();
                if (coinSystem != null)
                {
                    coinSystem.Gain(itemData.currencyValue); // Update the coin count
                }
                else
                {
                    Debug.LogError("CoinSystem not found in the scene.");
                }
            }

            // Destroy this GameObject (the item) after it has been collected
            Destroy(gameObject);
        }
        else
        {
            // Log an error message if the InventoryManager is not found
            Debug.LogError("InventoryManager is not assigned or not found.");
        }
    }
}