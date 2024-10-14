using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public int level;
    public int health;
    public float[] position;
    public PlayerData(PlayerController player)
    {
        level = player.GetLevel();
        health = player.GetHealth();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
    // Start is called before the first frame update
  
}
