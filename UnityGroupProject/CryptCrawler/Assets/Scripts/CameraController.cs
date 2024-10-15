using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField]
    int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
      //  Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {

        //get input
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        //invert Y camera position
        if (invertY)
            rotX += mouseY;
        else rotX -= mouseY;
        //clamp the rotX on the x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);
        //rotate the camera based on the input
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        //rotate the player based on the y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
} 
