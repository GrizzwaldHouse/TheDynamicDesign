using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// This script represents an item slot in the game.
public class ItemSlot : MonoBehaviour, IDropHandler
{
    // A property that returns the game object currently occupying this item slot.
    public GameObject Item
    {
        get
        {
            // Check if there is a child game object in this item slot.
            if (transform.childCount > 0)
            {
                // If so, return the first child game object.
                return transform.GetChild(0).gameObject;
            }
            // If not, return null.
            return null;
        }
    }

    // Called when a game object is dropped onto this item slot.
    public void OnDrop(PointerEventData eventData)
    {
        // Log a message to the console to indicate that a drop operation has occurred.
        Debug.Log("OnDrop");

        // Check if this item slot is currently empty.
        if (!Item)
        {
            // If so, set the parent transform of the dropped game object to this item slot's transform.
            DragDrop.itemBeingDragged.transform.SetParent(transform);

            // Set the local position of the dropped game object to zero.
            DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;
        }
    }
}