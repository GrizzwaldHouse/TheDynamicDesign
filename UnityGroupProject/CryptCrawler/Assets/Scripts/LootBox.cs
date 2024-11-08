using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


/*
 * This LootBox script is attached to the treasure box prefab.
 * All you have to do is drag the prefab onto the scene and
 * in the inspector, look for the Box Contents list under the Loot Box (script)
 * component. You add game object to this list and set the Drop chance (0.0f - 1f).
 * The higher the number the higher the chance that specific item will drop.
 */

[Serializable]
public class Loot
{

    public GameObject loot;


    [Range(0, 1)] public float dropChance;
}


public enum OpeningMethods { OpenOnCollision, OpenOnKeyPress, OpenOnTouch }


public class LootBox : MonoBehaviour
{

    public OpeningMethods openingMethod;
    public string playerTag = "Player";
    public KeyCode keyCode = KeyCode.E;
    public bool bouncingBox = true;
    public bool closeOnExit;
    public List<Loot> boxContents = new List<Loot>();
    private bool isPlayerAround;
    public bool isOpen { get; set; }
    Animator animator;
    [SerializeField] private Vector3 lootSpawnOffset = new Vector3(0, 1.0f, 1.5f);
    [SerializeField] private TMP_Text interactionText;
    private Transform playerTransform;
    public event Action<GameObject[]> OnBoxOpen;

    // Start is called before the first frame update
    void Start()
    {
        // gets the animator
        animator = GetComponent<Animator>();

        // set the animation to bounce or not
        BounceBox(bouncingBox);

        if (interactionText)
        {
            interactionText.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        // when player is close enough to open the box
        if (isPlayerAround)
        {
            FacePlayer();
            // in case of Key Press method for opening the box,
            // waits for a key to be pressed
            if(openingMethod == OpeningMethods.OpenOnKeyPress && Input.GetKey(keyCode))
            {
                Open();
            }
        }
    }

    private void FacePlayer()
    {
        if(interactionText && playerTransform)
        {
            interactionText.transform.LookAt(playerTransform);
            interactionText.transform.Rotate(0, 180, 0);
        }
    }


    public void BounceBox(bool bounceIt)
    {
        // flag the animator property "bounce" accordingly
        if (animator) animator.SetBool("bounce", bounceIt);
    }


    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        if (interactionText) interactionText.gameObject.SetActive(false);

        if (animator) animator.Play("Open");

        GameObject selectedLoot = null;

        // Check each item in the boxContents list for a chance to drop
        foreach (Loot loot in boxContents)
        {
            float chance = UnityEngine.Random.Range(0.0f, 1.0f);
            if (loot.dropChance >= chance)
            {
                selectedLoot = loot.loot;
                break; // Stop after selecting the first item that meets the drop chance
            }
        }

        // If no item was selected by chance, guarantee one random item
        if (selectedLoot == null && boxContents.Count > 0)
        {
            selectedLoot = boxContents[UnityEngine.Random.Range(0, boxContents.Count)].loot;
        }

        // Instantiate the selected loot item at a position in front of the chest
        if (selectedLoot != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 3.0f + Vector3.up * 0.5f;
            Instantiate(selectedLoot, spawnPosition, Quaternion.identity);
        }

        boxContents.Clear();
        OnBoxOpen?.Invoke(new GameObject[] { selectedLoot });
    }



    public void Close()
    {
        // avoid closing when it's already open
        if (!isOpen) return;
        isOpen = false;

        // play the close animation
        if (animator) animator.Play("Close");
    }


    private void OnMouseDown()
    {
        // checks if the opening method is OpenOnTouch
        if (openingMethod != OpeningMethods.OpenOnTouch) return;

        // Open the box.
        Open();
    }


    private void OnTriggerEnter(Collider collider)   {
        // OnCollisionMethod is not for OpenOnTouch method
        if (openingMethod == OpeningMethods.OpenOnTouch) return;

        // check if the hitting object is our player
        if (collider.gameObject.CompareTag(playerTag) && boxContents.Count > 0)
        {
            playerTransform = collider.transform;
            isPlayerAround = true;

            if (interactionText)
            {
                interactionText.gameObject.SetActive(true);
            }
            // if the method is OpenOnKeyPress, let's just flag the player as close
            if (openingMethod == OpeningMethods.OpenOnKeyPress) isPlayerAround = true;

            // otherwise, open the box.
            else Open();
        }
    }


    private void OnTriggerExit(Collider collider)
    {
        // flag the player as away.
        isPlayerAround = false;

        if (interactionText)
        {
            interactionText.gameObject.SetActive(false);
        }

        // if the box is suppose to close on exit, close it
        if (closeOnExit) Close();
    }
}
