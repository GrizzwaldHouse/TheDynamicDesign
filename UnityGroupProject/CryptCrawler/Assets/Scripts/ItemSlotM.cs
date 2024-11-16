using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotM : MonoBehaviour
{
    //item data
public ItemData currentItemData;

 public int currentQuantity;


    //item slot
    //  [SerializeField] private TMP_Text quantityText;
    //[SerializeField] public Image itemImage;


    public void AddItem(ItemData itemData, int quantity)
    {

        if (itemData == null)
        {
            return;
        }

        if (currentItemData == null)
        {
            currentItemData = itemData;
            currentQuantity = quantity;
        }

        else  (currentItemData == itemData && itemData.isStackable)
        {
            currentQuantity = Mathf.Min(currentQuantity + quantity, itemData.maxStackSize);
        }
        //currentQuantity += quantity;
        //int newQuantity = currentQuantity;

        //if (newQuantity <= itemData.maxStackSize)
        //{
        //    currentQuantity = newQuantity;

        //}
        //else
        //{
        //    int overFlow = newQuantity - itemData.maxStackSize;
        //    currentQuantity = itemData.maxStackSize;
        //}

       
    }
    public void ClearSlot()
    {
        currentItemData= null;
        currentQuantity = 0;
      
    }
}


    
