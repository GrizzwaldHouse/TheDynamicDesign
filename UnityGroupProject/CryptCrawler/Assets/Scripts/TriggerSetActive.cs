using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSetActive : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private ParticleSystem particleEffect;
    private MeshRenderer targetMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (targetGameObject != null)
        {
            targetMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
        }
        if (particleEffect != null)
        {
            particleEffect.Stop();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetMeshRenderer != null)
            {
                targetMeshRenderer.enabled = true;
            }
            if (particleEffect != null)
            {
                particleEffect.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetMeshRenderer != null)
        {
            // Deactivate the MeshRenderer
            targetMeshRenderer.enabled = false;
        }

        // Stop the ParticleSystem
        if (particleEffect != null)
        {
            particleEffect.Stop();
        }
    }
}

