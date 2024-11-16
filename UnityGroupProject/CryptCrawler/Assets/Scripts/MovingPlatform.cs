/*
 * MovingPlatform.cs
 * 
 * Author: [Marcus Daley]
 * Date: [Creation Date: 10-12-2024]
 * 
 * Description:
 * This script controls a platform that can move between multiple destinations along a specified axis or vertically.
 * It supports looping movement, collision reset, and can optionally use an easing function for smooth movement.
 * The platform can carry the player and reset position upon collision if desired.
 * 
 * Variables:
 * - _speed (float): Speed of horizontal platform movement.
 * - _verticalSpeed (int): Speed of vertical movement.
 * - _loopMovement (bool): If true, the platform will loop its movement between destinations.
 * - _resetOnCollision (bool): If true, the platform resets to its initial position on player collision.
 * - _useVerticalMovement (bool): Determines whether the platform moves vertically or along a specified axis.
 * - _movementDirection (float): Controls the direction of movement (1 for forward, -1 for backward).
 * - _destinations (List<Vector3>): List of destinations the platform moves between.
 * 
 * Events:
 * - OnStartMoving: Invoked when the platform starts moving.
 * - OnStopMoving: Invoked when the platform stops moving.
 * 
 * Usage:
 * - Attach this script to a platform GameObject with a Rigidbody and Collider component set to "Is Trigger".
 * - Define destinations in the Inspector or through code to specify the platform's movement path.
 * - Enable _loopMovement if continuous movement is needed.
 * - Adjust _speed and _verticalSpeed in the Inspector to set movement speed.
 * 
 * Dependencies:
 * - UnityEngine
 * - System
 * - System.Collections.Generic
 * 
 * Notes:
 * - Ensure the player object has a tag of "Player" for proper collision detection.
 * - Adjust easing function if a different motion smoothing is desired.
 */
using UnityEngine; // Importing UnityEngine namespace for Unity-specific functionality
using System; // Importing System namespace for using Action delegate
using System.Collections.Generic; // Importing Generic collections for using List<T>

public class MovingPlatform : MonoBehaviour // Class definition for MovingPlatform, inheriting from MonoBehaviour
{
    // Movement Settings
    [SerializeField]
    private float _speed ; // Speed of horizontal platform movement, adjustable in the Unity Inspector

    [SerializeField]
    private int _verticalSpeed; // Speed of vertical movement (integer), adjustable in the Unity Inspector

    // Movement Behavior
    [SerializeField]
    private bool _loopMovement ; // Determines if the platform should loop its movement between destinations

    [SerializeField]
    private bool _resetOnCollision ; // Determines if the platform should reset its position upon player collision

    [SerializeField]
    private bool _useVerticalMovement ; // Determines if the platform should move vertically (up and down)

    // Destination Points
    [SerializeField]
    private List<Transform> _destinations = new List<Transform>(); // List of destination points for the platform to move between

    // Internal State
    private int _currentDestinationIndex ; // Index of the current destination in the _destinations list
    private bool _isMoving = false; // Flag to indicate if the platform is currently moving
    private float _movementDirection ; // Direction of movement: 1 for forward, -1 for backward

    // Events to notify when the platform starts or stops moving
    public event Action OnStartMoving; // Event triggered when the platform starts moving
    public event Action OnStopMoving; // Event triggered when the platform stops moving
    private Vector3 _velocity = Vector3.zero; // Variable to store the current velocity
    void Start() // Unity's Start method, called before the first frame update
    {
       }

    void OnTriggerEnter(Collider other) // Unity's method called when another collider enters the trigger collider attached to this GameObject
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            _isMoving = true; // Set the moving flag to true, indicating the platform should start moving
           // Debug.Log("Player entered the platform. Starting movement."); // Log a message for debugging

            // If reset on collision is enabled, reset the platform's position
            if (_resetOnCollision)
            {
                ResetPlatformPosition(); // Call the method to reset the platform's position
               // Debug.Log("Platform reset to starting position due to collision."); // Log a message for debugging
            }

            // Set the player as a child of the platform, so it moves with the platform
            other.transform.SetParent(transform);
            StartMoving(); // Call the method to trigger the start moving event
        }
    }

    void OnTriggerExit(Collider other) // Unity's method called when another collider exits the trigger collider
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Remove the player as a child of the platform when they exit
            other.transform.SetParent(null);
          //  Debug.Log("Player exited the platform."); // Log a message for debugging
        }
    }

    void Update() // Unity's Update method, called once per frame
    {
        // If the platform is moving, call the MovePlatform method to update its position
        if (_isMoving)
        {
            MovePlatform(); // Call the method to move the platform
        }
    }

    private void MovePlatform() // Method to handle the movement of the platform
    {
        // Ensure there are at least two points to move between; if not, exit the method
        if (_destinations.Count < 2)
        {
          //  Debug.LogWarning("Not enough destinations set for platform movement."); // Log a warning if not enough destinations are set
            return; // Exit the method
        }

        // Calculate the target position based on the current destination
        Transform target = _destinations[_currentDestinationIndex]; // Get the current target destination
        Vector3 targetPosition = target.position; // Get the position of the target destination

        // Smoothly interpolate the platform's position towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 0.3f); // 0.3f is the smooth time

        // Check if the platform has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Update the destination index for the next move
            _currentDestinationIndex = (_currentDestinationIndex + 1) % _destinations.Count; // Loop back to the first destination if at the end
           // Debug.Log("Reached destination: " + target.name); // Log the reached destination for debugging

            // If looping is disabled and the platform has reached the last destination, stop moving
            if (!_loopMovement && _currentDestinationIndex == 0)
            {
                _isMoving = false; // Stop the platform from moving
                OnStopMoving?.Invoke(); // Trigger the stop moving event
               // Debug.Log("Platform stopped moving."); // Log a message for debugging
            }
        }

        // Handle vertical movement if enabled
        if (_useVerticalMovement)
        {
            // Calculate the vertical offset using a sine wave function
            float verticalOffset = Mathf.Sin(Time.time * _verticalSpeed) * 0.5f; // Adjust the multiplier for height
            Vector3 currentPosition = transform.position; // Get the current position of the platform

            // Update the platform's position to include the vertical offset
            currentPosition.y += verticalOffset; // Apply the vertical offset to the Y position
            transform.position = currentPosition; // Update the platform's position
        }
    }

    private void ResetPlatformPosition() // Method to reset the platform's position to the first destination
    {
       
        transform.position = _destinations[0].position; // Set the platform's position to the first destination
        _currentDestinationIndex = 0; // Reset the destination index
       // Debug.Log("Platform reset to starting position: " + _destinations[0]); // Log the reset position
    }

    private void StartMoving() // Method to trigger the start moving event
    {
        OnStartMoving?.Invoke(); // Invoke the OnStartMoving event if there are any subscribers
        //Debug.Log("Platform started moving."); // Log that the platform has started moving
    }

    private void StopMoving() // Method to stop the platform's movement
    {
        _isMoving = false; // Set the moving flag to false, indicating the platform should stop moving
        OnStopMoving?.Invoke(); // Invoke the OnStopMoving event if there are any subscribers
        Debug.Log("Platform stopped moving."); // Log that the platform has stopped moving
    }

    // Easing function (Quadratic ease-in-out) to smooth the movement
    private float EasingFunction(float t)
    {
        float result = t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t; // Calculate the eased value based on the input t
        //Debug.Log($"Easing function result for t={t}: {result}"); // Log the result of the easing function
        return result; // Return the eased value
    }
}