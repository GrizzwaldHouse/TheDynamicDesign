using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveCounter : MonoBehaviour
{
    int objectiveCounter = 0;

    public void ShowObjective()
    {
        GetComponent<TextMeshProUGUI>().text = $"Objective: {objectiveCounter} / {gamemanager.instance.accessQuestGiver.questObjectiveCount}";
    }

    public void ObjectiveIncrement()
    {
        objectiveCounter++;
        ShowObjective();
    }
}
