using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This script handles the drag and drop functionality for an item in the game.
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // The canvas component that this script is attached to.
    [SerializeField] private Canvas canvas;

    // The rect transform component that this script is attached to.
    private RectTransform rectTransform;

    // The canvas group component that this script is attached to.
    private CanvasGroup canvasGroup;

    // A static reference to the game object that is currently being dragged.
    public static GameObject itemBeingDragged;

    // A reference to the item slot that this script is attached to.
    public ItemSlot itemSlot;

    // The starting position of the game object when it is first dragged.
    Vector3 startPosition;

    // The starting parent transform of the game object when it is first dragged.
    Transform startParent;

    // Called when the script is initialized.
    private void Awake()
    {
        // Get the rect transform component attached to this game object.
        rectTransform = GetComponent<RectTransform>();

        // Get the canvas group component attached to this game object.
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Called when the drag operation begins.
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Log a message to the console to indicate that the drag operation has begun.
        Debug.Log("OnBeginDrag");

        // Set the alpha value of the canvas group to 0.6 to make the game object semi-transparent.
        canvasGroup.alpha = .6f;

        // Set the blocksRaycasts property of the canvas group to false to allow the game object to be dragged.
        canvasGroup.blocksRaycasts = false;

        // Store the starting position of the game object.
        startPosition = transform.position;

        // Store the starting parent transform of the game object.
        startParent = transform.parent;

        // Set the parent transform of the game object to the root transform to allow it to be dragged freely.
        transform.SetParent(transform.root);

        // Set the itemBeingDragged static reference to this game object.
        itemBeingDragged = gameObject;

        // Disable the mouse movement script while the game object is being dragged.
        GameObject mouseMovementObject = GameObject.Find("MouseMover");
        mouseMovementObject.GetComponent<MouseMovement>().enabled = false;
    }

    // Called when the drag operation ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        // Set the itemBeingDragged static reference to null.
        itemBeingDragged = null;

        // Check if the game object has been dropped back into its original position or into a valid item slot.
        if (transform.parent == startParent || transform.parent == transform.root)
        {
            // If so, reset the game object's position to its original position.
            transform.position = startPosition;

            // Reset the game object's parent transform to its original parent.
            transform.SetParent(startParent);
        }

        // Log a message to the console to indicate that the drag operation has ended.
        Debug.Log("OnEndDrag");

        // Reset the alpha value of the canvas group to 1 to make the game object fully visible again.
        canvasGroup.alpha = 1f;

        // Reset the blocksRaycasts property of the canvas group to true to block raycasts again.
        canvasGroup.blocksRaycasts = true;

        // Re-enable the mouse movement script now that the drag operation has ended.
        GameObject mouseMovementObject = GameObject.Find("MouseMover");
        mouseMovementObject.GetComponent<MouseMovement>().enabled = true;
    }

    // Called when the drag operation is in progress.
    public void OnDrag(PointerEventData eventData)
    {
        // Update the anchored position of the rect transform based on the drag delta.
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // This method is not used in this script, but it is included to satisfy the IDragHandler interface.
    public void OnDrop(PointerEventData eventData)
    {
        // Log a message to the console to indicate that the drop operation has occurred.
        Debug.Log("OnDrop");

        // Get the item slot component from the game object that was dropped onto.
        ItemSlot itemSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();

        // If the dropped game object is not an item slot, then open the inventory.
        if (!itemSlot)
        {
            // Set the parent transform of the dropped game object to the item slot transform.
            DragDrop.itemBeingDragged.transform.SetParent(itemSlot.transform);

            // Set the local position of the dropped game object to zero.
            DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;

            // Open the inventory.
            gamemanager.instance.OpenInventory();
        }
    }
}