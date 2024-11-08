using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Exposing a list of ItemData objects to the Unity Inspector for easy configuration in the editor.
    [SerializeField] private List<ItemData> shopItems = new List<ItemData>();

    // Exposing the CoinSystem instance to manage the player's currency.
    [SerializeField] private CoinSystem coinSystem;
    // Reference to ShopUI to update the inventory UI after buying or selling an item.
    [SerializeField] private ShopUI shopUI;

    public List<ItemData> GetShopItems()
    {
        return shopItems;
    }
    // Method to handle the purchase of an item from the shop.
    public void BuyItem(int itemIndex)
    {
        // Check if the itemIndex is valid (not negative and within the bounds of the shopItems list).
        if (itemIndex < 0 || itemIndex >= shopItems.Count) { return; }

        // Retrieve the item to buy from the shopItems list using the provided index.
        ItemData itemToBuy = shopItems[itemIndex];
        // Prevent buying currency items
        if (itemToBuy.category == ItemCategory.Currency)
        {
            Debug.Log("Cannot buy currency items!");
            return;
        }
        // Attempt to spend the currency value of the item using the CoinSystem.
        if (coinSystem != null && coinSystem.Spend(itemToBuy.currencyValue))
        {
            // If the purchase is successful, give the item stats to the player.
            PlayerController.instance.getItemStats(itemToBuy);
            // Log a message indicating the item was successfully bought.
            Debug.Log($"Bought {itemToBuy.menuName}");
            UpdateCoinDisplay();
            // Update the inventory UI to reflect the new item.
            shopUI.UpdateInventoryUI();
        }
        else
        {
            // Log a message indicating that the player does not have enough money to buy the item.
            Debug.Log("Not enough money!");
        }
    }

    // Method to handle the selling of an item from the player's inventory.
    public void SellItem(int itemIndex)
    {
        // Check if the itemIndex is valid (not negative and within the bounds of the player's item list).
        if (itemIndex < 0 || itemIndex >= PlayerController.instance.itemList.Count) return;

        // Retrieve the item to sell from the player's item list using the provided index.
        ItemData itemToSell = PlayerController.instance.itemList[itemIndex];
      // Prevent selling currency items
    if (itemToSell.category == ItemCategory.Currency)
        {
            Debug.Log("Cannot sell currency items!");
            return;
        }
        // Gain currency by adding the item's currency value to the CoinSystem.
        if (coinSystem.HasEnough(itemToSell.currencyValue))
        {
            coinSystem.Gain(itemToSell.currencyValue);

            // Remove the sold item from the player's inventory.
            PlayerController.instance.RemoveItemInventory(itemToSell);

            // Log a message indicating the item was successfully sold.
            Debug.Log($"Sold {itemToSell.menuName}");
            UpdateCoinDisplay();
            // Update the inventory UI to reflect the item removal.
            shopUI.UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Shopkeeper does not have enough currency to buy this item!");
        }
    }
    public void RenderInventoryItem(ItemData item)
    {
        if(item== null)
        {
            return;
            PlayerController.instance.getItemStats(item);
            // Log a message indicating the item was successfully bought.
            Debug.Log($"Bought {item.menuName}");
            UpdateCoinDisplay();
            
        }
    }
    private void UpdateCoinDisplay()
    {
        // This method can be used to refresh the coin display if needed
        coinSystem.UpdateCoin();
    }



}

