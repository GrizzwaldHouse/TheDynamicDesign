
using UnityEngine;
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int healingAmount;
    public int manaRAmount;
    public int healthBoosted;
    public int manaBoosted;
    public GameObject inventoryModel;
    [Header("Basic Information")]
    public string menuName;
  


    

    [Header("Stats")]
    public ItemCategory category;
    public int maxStackSize;
    public bool isStackable = true;
    //public float weight;
    [Header("Currency")]
    public int currencyValue;
}
public enum ItemCategory
{
    Weapon,
    Armor,
    Consumable,
    Material,
    QuestItem,
    Currency
}