using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class jumpPad : MonoBehaviour
{
    struct JumpPadTarget
    {
        public float contactTime;
        public Vector3 contactVelocity;
    }
    [SerializeField] Rigidbody rb;
    [SerializeField] float launchDelay ;
    [SerializeField] float launchForce ;
    [SerializeField] ForceMode launchMode = ForceMode.Impulse;
    [SerializeField] float impactVelocityScale ;
    [SerializeField] float maxImpactVelocityScale ;
    [SerializeField] float playerLaunchForceMultiplier ;
    [SerializeField] float maxDistortionWeight ;

    Dictionary<Rigidbody, JumpPadTarget> Targets = new Dictionary<Rigidbody, JumpPadTarget>();

    List<Rigidbody> targetsToClear =new List<Rigidbody>();

    private void FixedUpdate()
    {
        // Check for targets to launch
        float thresholdTime = Time.timeSinceLevelLoad - launchDelay;

        foreach(var kvp in Targets)
        {
            if(kvp.Value.contactTime >= thresholdTime)
            {
                Launch(kvp.Key, kvp.Value.contactVelocity);
                targetsToClear.Add(kvp.Key);
            }
        }

        // Clear out launched Targets
        foreach(var target in targetsToClear)
        {
            Targets.Remove(target);
        }
        targetsToClear.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Attempting to retrieve the rigid body
        Rigidbody rb;
        if (collision.gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            Targets[rb] = new JumpPadTarget()
            {
                contactTime = Time.timeSinceLevelLoad,
                contactVelocity = collision.relativeVelocity
            };

            // Prevent the player from passing through the wall
            Physics.IgnoreCollision(collision.collider, rb.GetComponent<Collider>(), true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        // Re-enable collision detection when the player exits the wall
        Physics.IgnoreCollision(collision.collider, collision.gameObject.GetComponent<Rigidbody>().GetComponent<Collider>(), false);
    }

    void Launch(Rigidbody targetRB, Vector3 contactVelocity)
    {
        Vector3 launchVector = transform.up;

        // Calculate the distortion vector
        Vector3 distortionVector = transform.forward * Vector3.Dot(contactVelocity, transform.forward) +
                                   transform.right * Vector3.Dot(contactVelocity, transform.right);

        launchVector = (launchVector + maxDistortionWeight * distortionVector.normalized).normalized;


        // Projects the relative velocity along the jump axis
        float contactProjection = Vector3.Dot(contactVelocity, transform.up);
        if (contactProjection < 0)
        {
            // Scale up the launch vector based on how fast we hit
            launchVector *= Mathf.Min(maxImpactVelocityScale, 1f + Mathf.Abs(contactProjection * impactVelocityScale));
        }

        if (targetRB.CompareTag("Player"))
            launchVector *= playerLaunchForceMultiplier;

        targetRB.AddForce(transform.up * launchForce, launchMode);
    }
}
