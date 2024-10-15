using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MouseMovement Script.
// This script controls the camera's rotation based on mouse input.
// It allows the player to look around the scene by moving the mouse.
public class MouseMovement : MonoBehaviour
{
    // Sensitivity of the camera's rotation.
    // A higher value makes the camera rotate faster.
    public float sensitivity = 10f;
    // Minimum vertical angle the camera can rotate.
    public float minAngle = -45f;
    // Maximum vertical angle the camera can rotate.
    public float maxAngle = 45f;

    // Current X-axis rotation of the camera.
    private float rotationX = 0f;
    // Current Y-axis rotation of the camera.
    private float rotationY = 0f;

    // Called when the script starts.
    private void Start()
    {
        // Locks the cursor to the center of the screen,
        // preventing it from moving outside the game window.
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // Called every frame.
    private void Update()
    {
        // Checks if an item is currently being dragged in the game.
        // This prevents camera rotation while dragging items.
        if (DragDrop.itemBeingDragged == null)
        {
            // Get the horizontal and vertical mouse movement input.
            // The sensitivity value is applied to scale the input.
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Update the camera's rotation based on the mouse input.
            rotationX += mouseX;
            rotationY -= mouseY;

            // Clamp the vertical rotation within the specified limits.
            rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);

            // Apply the calculated rotation to the camera's transform.
            // This rotates the camera around its local Y-axis.
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }
}