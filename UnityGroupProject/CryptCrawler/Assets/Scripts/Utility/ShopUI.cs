using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    // Reference to the ShopManager that handles shop logic
    [SerializeField] ShopManager shopManager;

    // Parent object to hold the buttons for buying and selling items
    [SerializeField] Transform buttonContainer;

    // Prefab for the buy button
    [SerializeField] GameObject buyButtonPrefab;

    // Prefab for the sell button
    [SerializeField] GameObject sellButtonPrefab;

    // Header for inventory UI settings
    [Header("Inventory UI")]

    // Parent object to hold the inventory slots
    [SerializeField] Transform inventoryContainer;

    // Prefab for the inventory slot
    [SerializeField] GameObject inventorySlotPrefab;
    private PlayerController playerController;
    // Awake is called when the script instance is being loaded

    // Start is called before the first frame update
    void Start()

    {
        Debug.Log("ShopUI Start called");// Attempt to get the PlayerController instance
        playerController = PlayerController.instance;
        // Check if the PlayerController instance is initialized
        if (PlayerController.instance == null)
        {
            Debug.LogError("PlayerController instance is not initialized!");
            return; // Exit if the PlayerController instance is null
        }
       
        // Update the inventory UI to reflect the current items
        UpdateInventoryUI();
    }

    // Method to populate the shop UI with buy and sell buttons
    public void PopulateShopUI()
    {
        // Debugging statements to identify null references
        Debug.Log($"shopManager: {shopManager}");
        Debug.Log($"buttonContainer: {buttonContainer}");
        Debug.Log($"buyButtonPrefab: {buyButtonPrefab}");
        Debug.Log($"sellButtonPrefab: {sellButtonPrefab}");

        // Check for null references and log an error if any are missing
        if (shopManager == null || buttonContainer == null || buyButtonPrefab == null || sellButtonPrefab == null)
        {
            Debug.LogError("One or more required references are missing in ShopUI!");
            return; // Exit the method if any reference is missing
        }

        // Clear existing buttons in the button container
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Verify the shop items list from the ShopManager
        var shopItems = shopManager.GetShopItems();
        if (shopItems == null)
        {
            Debug.LogError("ShopManager's item list is null.");
            return; // Exit if the shop items list is null
        }

        // Create buttons for each item in the shop
        for (int i = 0; i < shopItems.Count; i++)
        {
            CreateButton(i, false); // Create buy buttons for shop items
        }

        // Verify the player item list from the PlayerController
        if (PlayerController.instance == null || PlayerController.instance.itemList == null)
        {
            Debug.LogError("PlayerController instance or itemList is not initialized.");
            return; // Exit if the player item list is null
        }

        // Create buttons for each item in the player's inventory
        for (int i = 0; i < PlayerController.instance.itemList.Count; i++)
        {
            CreateButton(i, true); // Create sell buttons for player items
        }
    }

    // Method to create a button for buying or selling an item
    private void CreateButton(int itemIndex, bool isSellButton)
    {
        // Determine which button prefab to use based on the action (buy/sell)
        GameObject buttonPrefab = isSellButton ? sellButtonPrefab : buyButtonPrefab;

        // Check if the button prefab is assigned
        if (buttonPrefab == null)
        {
            Debug.LogError("Button prefab is not assigned");
            return; // Exit if the button prefab is not assigned
        }

        // Instantiate a new button from the prefab
        GameObject buttonObject = Instantiate(buttonPrefab, buttonContainer);

        // Set the button's item data based on whether it's a sell or buy button
        ItemData itemData = isSellButton ?
            (PlayerController.instance.itemList.Count > itemIndex ? PlayerController.instance.itemList[itemIndex] : null) :
            shopManager.GetShopItems().Count > itemIndex ? shopManager.GetShopItems()[itemIndex] : null;

        // Check if the item data is valid
        if (itemData == null)
        {
            Debug.LogError("Item data is null");
            Destroy(buttonObject); // Destroy the button if item data is null
            return; // Exit if item data is null
        }

        // Set the button text to reflect the item's name and action (buy/sell)
        SetButtonText(buttonObject, itemData, isSellButton);

        // Get the ButtonFunctions component from the button prefab
        ButtonFunctions buttonFunctions = buttonObject.GetComponent<ButtonFunctions>();
        if (buttonFunctions != null)
        {
            // Assign the shop manager and item index to the button functions
            buttonFunctions.shopManager = shopManager;
            buttonFunctions.itemIndex = itemIndex;
            buttonFunctions.isSellButton = isSellButton;

            // Add the OnButtonClick method as a listener for the button's click event
            buttonObject.GetComponent<Button>().onClick.AddListener(buttonFunctions.OnButtonClick);
        }
        else
        {
            Debug.LogError("ButtonFunctions component is missing on the button prefab.");
        }
    }

    // Method to set the text of the button based on the item data
    private void SetButtonText(GameObject buttonObject, ItemData itemData, bool isSellButton)
    {
        // Get the Text component and TextMeshProUGUI component from the button
        Text buttonText = buttonObject.GetComponentInChildren<Text>();
        TextMeshProUGUI tmpText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

        // Create the button label based on the action (buy/sell) and item name
        string buttonLabel = isSellButton ? "Sell " + itemData.menuName : "Buy " + itemData.menuName;

        // Set the button text if the Text component is found
        if (buttonText)
            buttonText.text = buttonLabel;
        else if (tmpText != null)
            tmpText.text = buttonLabel; // Set the text for TextMeshProUGUI if available
        else
        {
            Debug.LogWarning("Text component missing on button prefab.");
        }
    }

    // Method to update the inventory UI to reflect the current items
    public void UpdateInventoryUI()
    {
        // Clear existing inventory slots in the inventory container
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate the inventory UI with items from the player's inventory
        foreach (var item in PlayerController.instance.itemList)
        {
            // Instantiate a new inventory slot for each item
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);

            // Set the slot's text or icon to the item's data
            TextMeshProUGUI itemText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = item.menuName; // Set the item name in the slot
            }

            // Get the Image component to display the item icon
            Image itemIcon = slot.GetComponentInChildren<Image>();
            if (itemIcon != null && item.inventoryModel != null)
            {
                // Instantiate the item model using MeshFilter and MeshRenderer
                if (item.inventoryMeshFilter != null && item.inventoryMeshRenderer != null)
                {
                    GameObject itemModel = new GameObject(item.menuName); // Create a new GameObject for the item model
                    MeshFilter meshFilter = itemModel.AddComponent<MeshFilter>(); // Add MeshFilter component
                    MeshRenderer meshRenderer = itemModel.AddComponent<MeshRenderer>(); // Add MeshRenderer component

                    // Set the mesh and materials for the item model
                    meshFilter.sharedMesh = item.inventoryMeshFilter.sharedMesh; // Set the mesh
                    meshRenderer.sharedMaterials = item.inventoryMeshRenderer.sharedMaterials; // Set the materials

                    // Set the position of the item model in the inventory slot
                    itemModel.transform.SetParent(slot.transform);
                    itemModel.transform.localPosition = Vector3.zero; // Adjust position as needed
                }
                else
                {
                    Debug.LogWarning("MeshFilter or MeshRenderer is missing for item: " + item.menuName);
                }

                // Enable the icon to display it in the inventory slot
                itemIcon.enabled = true; // Show the item icon

                Debug.Log($"Inventory UI updated for item: {item.menuName}"); // Log the update for debugging
            }
            else
            {
                Debug.LogWarning("Image component missing on inventory slot prefab.");
            }

            Debug.Log($"Inventory UI updated for item: {item.menuName}"); // Log the update for each item
        }
    }
}