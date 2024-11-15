using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextLevel : MonoBehaviour
{
    [SerializeField] private string loadLevel;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!other.isTrigger)
        {
            gamemanager.instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    }
    
}
