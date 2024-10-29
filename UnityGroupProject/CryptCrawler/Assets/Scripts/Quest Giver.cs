using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Canvas questTextScreen;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] public GameObject menuActive;

    public bool hasQuest;
    bool playerInRange;

    void GiveQuest()
    {

        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
        {
            questManager.CreateQuest("Clear Cemetary", "Kill 10 skeletons.");
            questText.text = "Quest Accepted: Clear Cemetary";
        }
        menuActive = questTextScreen.gameObject;
        gamemanager.instance.accessPlayer.currentQuest = questText;
        menuActive.SetActive(true);
        gamemanager.instance.accessPlayer.hasQuest = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            GiveQuest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            menuActive.SetActive(false);
            playerInRange = false;
        }
    }
}
