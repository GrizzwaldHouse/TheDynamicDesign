using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
// This class handles the transition between different game areas when the player enters a trigger collider.
public class SectionTrigger : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the player's current quest.
    public TextMeshProUGUI playerQuest;

    // Name of the area transition, used for tracking the player's current area.
    public string areaTransitionName;

    // Name of the next scene to load when the player enters the trigger.
    public string nextSceneName;

    // Reference to the player's character GameObject, used to control its visibility and position.
    public GameObject playerChar;

    // Reference to the AreaEnterance component, which handles area transition logic.
    public AreaEnterance theEnterance;

    // Transform representing the destination position where the player will be teleported.
    public Transform destination;

    // This method is called when another collider enters the trigger collider attached to this GameObject.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the trigger is tagged as "Player".
        if (other.gameObject.CompareTag("Player"))
        {
            // Set the transition name in the AreaEnterance component to the current area's transition name.
            theEnterance.transitionName = areaTransitionName;

            // Assign the current quest to the player's quest tracker in the game manager.
            gamemanager.instance.accessPlayer.currentQuest = playerQuest;

            // Load the next scene specified by the nextSceneName variable.
            SceneManager.LoadScene(nextSceneName);

            // Deactivate the player character to prevent any movement or actions during the transition.
            playerChar.SetActive(false);

            // Move the player character to the destination position specified by the destinataion Transform.
            playerChar.transform.position = destination.position;

            // Reactivate the player character after moving it to the new position.
            playerChar.SetActive(true);
        }
    }
}