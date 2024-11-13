
using UnityEngine;
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    // Amount of health restored when this item is used
    public int healingAmount;

    // Amount of mana restored when this item is used
    public int manaRAmount;

    // Amount of health boosted when this item is equipped or used
    public int healthBoosted;

    // Amount of mana boosted when this item is equipped or used
    public int manaBoosted;

    // Reference to the GameObject that represents this item in the inventory
    public GameObject inventoryModel;

    // Header attribute adds a label in the Inspector for better organization
    [Header("Basic Information")]
    // Name of the item as it appears in the menu
    public string menuName;

    // Header attribute for organizing stats-related variables in the Inspector
    [Header("Stats")]
    // Category of the item, defined by the ItemCategory enum
    public ItemCategory category;

    // Maximum number of this item that can be stacked in the inventory
    public int maxStackSize;

    // Indicates whether this item can be stacked in the inventory
    public bool isStackable = true;

    // Uncomment the following line if weight is to be included in item stats
    // public float weight;

    // Header attribute for organizing currency-related variables in the Inspector
    [Header("Currency")]
    // Value of the item in terms of currency
    public int currencyValue;

    // Header attribute for organizing model-related variables in the Inspector
    [Header("Model")]
    // MeshFilter component for the item model, used to define the mesh that represents the item
    public MeshFilter inventoryMeshFilter; // MeshFilter for the item model

    // MeshRenderer component for the item model, used to render the mesh with materials
    public MeshRenderer inventoryMeshRenderer; // MeshRenderer for the item model
}

// Enum to categorize different types of items in the game
public enum ItemCategory
{
    Weapon,      // Represents items that can be used to deal damage
    Armor,       // Represents items that provide protection
    Consumable,  // Represents items that can be consumed for effects (e.g., potions)
    Material,    // Represents items used for crafting or building
    QuestItem,   // Represents items that are part of quests
    Currency     // Represents items that can be used as currency in the game
}