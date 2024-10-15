using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextLevel : MonoBehaviour
{
    [SerializeField] private string loadLevel;
   void TriggerNextLevel(Collider other)
    {
        if (other.tag == "NextLevel")
        {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
       


        
    }
}
