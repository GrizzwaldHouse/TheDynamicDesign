using UnityEngine;

//public class MovingPlatform : MonoBehaviour
//{

//    // Serialized fields
//    [SerializeField]
//    private float _moveDistance;

//    [SerializeField]
//    private float _speed;

//    [SerializeField]
//    private bool _loopMovement = true;

//    [SerializeField]
//    private bool _resetOnCollision = true;

//    // Private fields
//    private Vector3 _startPosition;
//    private bool _isMoving = false;
//    [SerializeField] private float _movementDirection;

//    // Getters and setters
//    public float MoveDistance
//    {
//        get { return _moveDistance; }
//        set { _moveDistance = value; }
//    }

//    public float Speed
//    {
//        get { return _speed; }
//        set { _speed = value; }
//    }

//    public bool LoopMovement
//    {
//        get { return _loopMovement; }
//        set { _loopMovement = value; }
//    }

//    public bool ResetOnCollision
//    {
//        get { return _resetOnCollision; }
//        set { _resetOnCollision = value; }
//    }

//    void Start()
//    {
//        _startPosition = transform.position;
//    }




//    void OnTriggerEnter(Collider other)
//    {
//        // Check if the character has landed on the platform
//        if (other.gameObject.CompareTag("Player"))
//        {
//            // Debug.Log("Player has landed on the platform!");
//            PlayerController.position = transform.position;
//            // Set the flag to move the platform
//            _isMoving = true;

//            // Set the movement direction to 1 (right)
//            _movementDirection = 1.0f;

//            // If reset on collision is true, reset the platform position
//            if (_resetOnCollision)
//            {
//                transform.position = _startPosition; // Reset platform position
//            }
//        }
//    }




//    void Update()
//    {
//        if (_isMoving)
//        {
//            // Move the platform left and right
//            float newX = Mathf.Lerp(_startPosition.x, _startPosition.x + _moveDistance * _movementDirection, Mathf.PingPong(Time.time * _speed, 1.0f));
//            transform.position = new Vector3(newX, _startPosition.y, _startPosition.z);

//            // Check if the platform has reached the end of its movement
//            if (Mathf.Abs(transform.position.y - _startPosition.z) >= _moveDistance)
//            {
//                if (_loopMovement)
//                {
//                    // Reverse movement direction
//                    _movementDirection = -_movementDirection;
//                }
//                else
//                {
//                    _isMoving = false; // Stop moving
//                }
//            }
//        }
//    }
//}
