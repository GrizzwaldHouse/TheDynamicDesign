using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float sensitivity = 10f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (DragDrop.itemBeingDragged == null)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;

            rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);

            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }
}