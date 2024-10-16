using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Serialized fields: These variables can be edited in the Unity Inspector
    [SerializeField]
    private float _moveDistance; // The distance the platform will move from its starting position

    [SerializeField]
    private float _speed; // The speed at which the platform will move

    [SerializeField]
    private bool _loopMovement = true; // Whether the platform will loop back to its starting position after reaching the end of its movement

    [SerializeField]
    private bool _resetOnCollision = true; // Whether the platform will reset to its starting position when the player collides with it

    // Private fields: These variables are used internally by the script
    private Vector3 _startPosition; // The initial position of the platform
    private bool _isMoving = false; // A flag indicating whether the platform is currently moving
    [SerializeField]
    private float _movementDirection; // The direction of the platform's movement (1 for right, -1 for left)
    private Rigidbody _rb; // The Rigidbody component attached to the platform

    // Getters and setters: These allow other scripts to access and modify the serialized fields
    public float MoveDistance
    {
        get { return _moveDistance; } // Returns the current move distance
        set { _moveDistance = value; } // Sets a new move distance
    }

    public float Speed
    {
        get { return _speed; } // Returns the current speed
        set { _speed = value; } // Sets a new speed
    }

    public bool LoopMovement
    {
        get { return _loopMovement; } // Returns whether the platform will loop back to its starting position
        set { _loopMovement = value; } // Sets whether the platform will loop back to its starting position
    }

    public bool ResetOnCollision
    {
        get { return _resetOnCollision; } // Returns whether the platform will reset to its starting position on collision
        set { _resetOnCollision = value; } // Sets whether the platform will reset to its starting position on collision
    }

    void Start()
    {
        // Store the initial position of the platform
        _startPosition = transform.position;

        // Get the Rigidbody component attached to the platform
        _rb = GetComponent<Rigidbody>();

        // Make sure the Rigidbody is not kinematic
        _rb.isKinematic = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the character has landed on the platform
        if (other.gameObject.CompareTag("Player"))
        {
          

            // Set the flag to move the platform
            _isMoving = true; // Indicate that the platform should start moving

            // Set the movement direction to 1 (right)
            _movementDirection = 1.0f; // Initialize the movement direction to move the platform to the right

            // If reset on collision is true, reset the platform position
            if (_resetOnCollision)
            {
                transform.position = _startPosition; // Reset the platform position to its initial position
            }
        }
    }

    void Update()
    {
        if (_isMoving)
        {
            // Move the platform left and right using a ping-pong motion
            float newX = Mathf.Lerp(_startPosition.x, _startPosition.x + _moveDistance * _movementDirection, Mathf.PingPong(Time.time * _speed, 1.0f));
            Vector3 targetPosition = new Vector3(newX, _startPosition.y, _startPosition.z); // Calculate the target position

            // Calculate the velocity needed to reach the target position
            Vector3 velocity = (targetPosition - transform.position) / Time.deltaTime;

            // Set the velocity of the Rigidbody
            _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, _rb.velocity.z); // Only update the x-component of the velocity

        }
    }
}