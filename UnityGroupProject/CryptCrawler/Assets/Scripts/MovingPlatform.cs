using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Serialized fields: These variables can be edited in the Unity Inspector
    [SerializeField]
    private float changeDirectionDelay; // The distance the platform will move from its starting position

    [SerializeField]
    private float _speed; // The speed at which the platform will move
    [SerializeField] Transform startPoint, endPoint;
    private Transform destinationTarget, departTarget;
    [SerializeField]private float startTime;
   [SerializeField] private float journeyLength;
    bool isWaiting;

    private void Start()
    {
        departTarget = startPoint;
        destinationTarget = endPoint;
        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (!isWaiting)
        {


            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                float distCoved = (Time.time - startTime) * _speed;
                float fractionOfJourney = distCoved / journeyLength;
                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(changeDelay());
            }
        }
       
    }
    void ChangeDestination()
    {
        if (departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }
    }
    IEnumerator changeDelay()
    {
        yield return new WaitForSeconds(changeDirectionDelay);

        ChangeDestination();
        startTime = Time.time;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            other.transform.parent = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }
}