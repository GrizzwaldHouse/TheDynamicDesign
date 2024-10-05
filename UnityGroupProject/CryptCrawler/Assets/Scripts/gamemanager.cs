using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    public static gamemanager instance;

    public bool isPaused;
    float timeScaleOrig;

    void Start()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
       
    }
     void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }
}

