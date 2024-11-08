using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionTrigger : MonoBehaviour
{
    public TextMeshProUGUI playerQuest;
    public string nextSceneName; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
            //gamemanager.instance.questText = playerQuest;
            //gamemanager.instance.accessPlayer.currentQuest = playerQuest;
        }
    }
}
