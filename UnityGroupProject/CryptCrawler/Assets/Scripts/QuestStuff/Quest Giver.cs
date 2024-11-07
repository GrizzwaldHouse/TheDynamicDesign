 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Canvas questTextScreen;
    [SerializeField] public TextMeshProUGUI questText;
    [SerializeField] public GameObject menuActive;
    [SerializeField] public int questObjectiveCount;
    [SerializeField] string QuestName;
    public UnityEvent GetQuest;

    [SerializeField] public string QuestEnemyToBeat;

    public bool hasQuest;
    bool playerInRange;

    void GiveQuest()
    {
        menuActive = questTextScreen.gameObject;
        gamemanager.instance.accessPlayer.currentQuest = questText;
        gamemanager.instance.accessSectionTrigger.playerQuest = questText;
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
            GetQuest.Invoke();
        }
    }
}
