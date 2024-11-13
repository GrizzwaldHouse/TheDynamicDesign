using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeated : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    
    [SerializeField] List<GameObject>teleporterObjects;

    private void OnDestroy()
    {
        SetActiveState(targetObject, true);
        SetActiveTeleporterStates(true);
    }

    private void SetActiveState(GameObject targetObject, bool state)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(state);
        }
    }
    private void SetActiveTeleporterStates(bool state )
    {
        foreach (GameObject teleporter in teleporterObjects)
        {
            if (teleporter != null)
            {
                SetActiveState(teleporter, state);
            }
        }
    }
}
