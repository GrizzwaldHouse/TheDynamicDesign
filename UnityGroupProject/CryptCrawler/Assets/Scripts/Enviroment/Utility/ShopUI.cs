using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;// Reference to the ShopManager.
    [SerializeField] Transform buttonContainer; // Parent object to hold the buttons.
    [SerializeField] GameObject buyButtonPrefab; // Prefab for the buy button
    [SerializeField] GameObject sellButtonPrefab;
    [Header("Inventory UI")]
    [SerializeField] Transform inventoryContainer; // Parent object to hold the inventory items
    [SerializeField] GameObject inventorySlotPrefab; // Prefab for the inventory slot
    void Start()
    {
        PopulateShopUI();
        UpdateInventoryUI();
    }

    private void PopulateShopUI()
    {
        // Create exiting buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        //Create buttons for each in the shop
        for (int i = 0; i < shopManager.GetShopItems().Count; i++)
        {
            CreateButton(i, false);
        }
        for (int i = 0; i < PlayerController.instance.itemList.Count; i++)
        {
            CreateButton(i, true);
        }
    }
    private void CreateButton(int itemIndex, bool isSellButton)
    {
        GameObject buttonPrefab = isSellButton ? sellButtonPrefab : buyButtonPrefab;

        if (buttonPrefab == null)
        {
            Debug.LogError("Button prefab is not assigned");
            return;
        }//Instanttiate a new buttonfrom the prefab
        GameObject buttonObject = Instantiate(buttonPrefab, buttonContainer);
        //Set the buttons the to the items name
        ItemData itemData = isSellButton ? (PlayerController.instance.itemList.Count > itemIndex ? PlayerController.instance.itemList[itemIndex] : null) :
            shopManager.GetShopItems().Count > itemIndex ? shopManager.GetShopItems()[itemIndex] : null;
        if (itemData == null)
        {
            Debug.LogError("Item data is null");
            Destroy(buttonObject);
            return;
        }

        SetButtonText(buttonObject, itemData, isSellButton);
        ButtonFunctions buttonFunctions = buttonObject.GetComponent<ButtonFunctions>();
        if (buttonFunctions != null)
        {
            buttonFunctions.shopManager = shopManager;
            buttonFunctions.itemIndex = itemIndex;
            buttonFunctions.isSellButton = isSellButton;
            buttonObject.GetComponent<Button>().onClick.AddListener(buttonFunctions.OnButtonClick); // Set the button's name to the item's menu name
        }
        else
        {
            Debug.LogError("ButtonFunctions component is missing on the button prefab.");
        }
    }
    private void SetButtonText(GameObject buttonObject, ItemData itemData, bool isSellButton)
    {

        Text buttonText = buttonObject.GetComponentInChildren<Text>();
        TextMeshProUGUI tmpText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        string buttonLabel = isSellButton ? "Sell " + itemData.menuName : "Buy " + itemData.menuName;
        if (buttonText)
            buttonText.text = buttonLabel;
        else if (tmpText != null)
            tmpText.text = buttonLabel;
        else
        {
            Debug.LogWarning("Text component missing on button prefab.");
        }
    }
    public void UpdateInventoryUI()
    {
        // Clear existing inventory slots
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate the inventory UI with items from the player's inventory
        foreach (var item in PlayerController.instance.itemList)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);

            // Set the slot's text or icon to the item's data
            TextMeshProUGUI itemText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = item.menuName;
            }


            Image itemIcon = slot.GetComponentInChildren<Image>();
            if (itemIcon != null && item.inventoryModel != null)
            {
                // Instantiate the item model using MeshFilter and MeshRenderer
                if (item.inventoryMeshFilter != null && item.inventoryMeshRenderer != null)
                {
                    GameObject itemModel = new GameObject(item.menuName);
                    MeshFilter meshFilter = itemModel.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = itemModel.AddComponent<MeshRenderer>();

                    meshFilter.sharedMesh = item.inventoryMeshFilter.sharedMesh; // Set the mesh
                    meshRenderer.sharedMaterials = item.inventoryMeshRenderer.sharedMaterials; // Set the materials

                    // Set the position of the item model in the inventory slot
                    itemModel.transform.SetParent(slot.transform);
                    itemModel.transform.localPosition = Vector3.zero; // Adjust as needed
                }
                else
                {
                    Debug.LogWarning("MeshFilter or MeshRenderer is missing for item: " + item.menuName);

                }
                // Set the icon if it exists
                itemIcon.enabled = true; // Enable the icon

                Debug.Log($"Inventory UI updated for item: {item.menuName}");
            }
            else
            {
                Debug.LogWarning("Image component missing on inventory slot prefab.");
            }

            Debug.Log($"Inventory UI updated for item: {item.menuName}");
        }
    
    }
}
