using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject nextSection;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(nextSection,new Vector3(159,0,4),Quaternion.identity);
        }
    }
}
