using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionTrigger : MonoBehaviour
{
    
   
    public TextMeshProUGUI playerQuest;
    public string areaTransitionName;
    public string nextSceneName;
    public GameObject playerChar;
    public AreaEnterance theEnterance;
    public Transform destinataion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            theEnterance.transitionName = areaTransitionName;
            gamemanager.instance.accessPlayer.currentQuest = playerQuest;
           SceneManager.LoadScene(nextSceneName);
            gamemanager.instance.accessPlayer.currentQuest = playerQuest;
            playerChar.SetActive(false);
            //PlayerController.instance.areaTransitionName = areaTransitionName;
            playerChar.transform.position = destinataion.position;
            playerChar.SetActive(true);
        }
    }
}
