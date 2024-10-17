using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timerscript : MonoBehaviour
{
    [SerializeField] float startTime;
    float currTime;
    bool startedTime = false;
    [SerializeField] TMP_Text timerText;



    // Start is called before the first frame update
    void Start()
    {
        currTime = startTime;
        timerText.text = currTime.ToString("f2");
        startedTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedTime)
        {
            currTime += Time.deltaTime;
        }
        timerText.text = currTime.ToString("f2");
    }
}
