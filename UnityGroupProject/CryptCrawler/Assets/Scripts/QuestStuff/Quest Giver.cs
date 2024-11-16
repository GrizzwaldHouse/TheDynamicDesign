 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestGiver : MonoBehaviour, IQuestManager
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
    private void Start()
    {
        if (gamemanager.instance.accessPlayer.hasQuest)
        {
            CreateQuest(gamemanager.instance.accessPlayer.currentQuestName, questText.text);
        }
    }
    void GiveQuest()
    {
        CreateQuest(QuestName, questText.text);
        menuActive = questTextScreen.gameObject;
        gamemanager.instance.accessPlayer.currentQuest = questText;
     //   gamemanager.instance.accessSectionTrigger.playerQuest = questText;
        menuActive.SetActive(true);
        gamemanager.instance.accessPlayer.hasQuest = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            GiveQuest();
            gamemanager.instance.accessPlayer.playerQuest = gamemanager.instance.accessPlayer.currentQuest;
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

    public void CreateQuest(string questName, string description)
    {
        questText.text = $"{questName}: {description}"; // Display the quest name and description
    }

    public void CompleteQuest()
    {
        throw new System.NotImplementedException();
    }

    public void AddSpawner(ObjectSpawner spawner)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveSpawner(ObjectSpawner spawner)
    {
        throw new System.NotImplementedException();
    }

    public void NotifyObjectSpawned(ObjectSpawner spawner)
    {
        throw new System.NotImplementedException();
    }
}
