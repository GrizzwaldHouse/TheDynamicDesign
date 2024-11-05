using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotM : MonoBehaviour
{
    //item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;



    //item slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] public Image itemImage;


    public void AddItem(string itemName, int quantity, Sprite itemSPrite)
    {
        this.itemName = itemName; 
        this.quantity = quantity;
        this.itemSprite = itemSPrite;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSPrite;  
    }
}
