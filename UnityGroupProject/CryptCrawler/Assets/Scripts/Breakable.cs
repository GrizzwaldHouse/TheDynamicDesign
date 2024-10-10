using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a class called Destroywall that inherits from MonoBehaviour and implements the IDamage interface
public class Destroywall : MonoBehaviour, IDamage
{
    // Serialize a Renderer component called model, which can be set in the Unity Inspector
    [SerializeField] Renderer model;

    // Serialize an integer variable called durablity, which can be set in the Unity Inspector
    [SerializeField] int durablity;

    // Declare a Color variable to store the original color of the model
    Color colorOrig;

    // The Start method is called before the first frame update
    void Start()
    {
        // Store the original color of the model's material
        colorOrig = model.material.color;
    }

    // The Update method is called once per frame, but it's empty in this case
    void Update()
    {
        // No code here, so this method does nothing
    }

    // Implement the takeDamage method from the IDamage interface
    public void takeDamage(int amount)
    {
        // Reduce the durablity by the amount of damage taken
        durablity -= amount;

        // Start a coroutine to flash the color of the model
        StartCoroutine(flashColor());

        // If the durablity is 0 or less, destroy the game object
        if (durablity <= 0)
        {
            // Destroy the game object this script is attached to
            Destroy(gameObject);
        }
    }

    // Define a coroutine to flash the color of the model
    IEnumerator flashColor()
    {
        // Set the color of the model's material to magenta
        model.material.color = Color.magenta;

        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.1f);

        // Set the color of the model's material back to its original color
        model.material.color = colorOrig;
    }
}
