using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a public class called "MazeCellObject" that inherits from MonoBehaviour
public class MazeCellObject : MonoBehaviour
{
    // Declare four serialized GameObject variables: topWall, bottomWall, leftWall, and rightWall
    // These will be set in the Unity editor to specify the game objects that represent the walls of a maze cell
    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;

    // Define a public void method called "Init" that takes four boolean parameters: top, bottom, right, and left
    public void Init(bool top, bool bottom, bool right, bool left)
    {
        // Set the top wall game object to be active or inactive based on the top parameter
        topWall.SetActive(top);

        // Set the bottom wall game object to be active or inactive based on the bottom parameter
        bottomWall.SetActive(bottom);

        // Set the left wall game object to be active or inactive based on the left parameter
        leftWall.SetActive(left);

        // Set the right wall game object to be active or inactive based on the right parameter
        rightWall.SetActive(right);
    }
}
