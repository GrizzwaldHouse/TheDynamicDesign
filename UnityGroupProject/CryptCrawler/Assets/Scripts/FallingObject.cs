using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; // Speed at which the object will move towards the platform

    [SerializeField]
    private float lifetime; // Time in seconds before the object is destroyed
    private bool isMoving; // Flag to check if the object should move
    private void Start()
    {
        // Start the coroutine to destroy the object after a set lifetime
        StartCoroutine(DestroyAfterTime(lifetime));
    }
    private void Update()
    {
        if (isMoving)
        {
            // Move towards the target position (the platform)
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);
        }
    }

    public void StartMoving(Vector3 targetPosition)
    {
        isMoving = true;
        // Set the target position to the platform's position
        Vector3 target = targetPosition;
        // Move towards the target position
        StartCoroutine(MoveTowardsTarget(target));
    }
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy the object after the specified time
    }
    private IEnumerator MoveTowardsTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        // Stop moving once the target is reached
        isMoving = false;
    }
}
