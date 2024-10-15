using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public static GameObject itemBeingDragged;
    public ItemSlot itemSlot;
    Vector3 startPosition;
    Transform startParent;
    
    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void onBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }
    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        ItemSlot itemSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>();
        if (!itemSlot)
        {
            DragDrop.itemBeingDragged.transform.SetParent(itemSlot.transform);
            DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;
            //Open the inventory
            gamemanager.instance.OpenInventory();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // Re-enable MouseMovement after dragging
        GameObject mouseMovementObject = GameObject.Find("MouseMover");
        mouseMovementObject.GetComponent<MouseMovement>().enabled = true;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

        // Disable MouseMovement while dragging
        GameObject mouseMovementObject = GameObject.Find("YourMouseMovementObject");
        mouseMovementObject.GetComponent<MouseMovement>().enabled = false;

    }
}