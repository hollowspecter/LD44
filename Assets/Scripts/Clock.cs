using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static Clock instance;

    public float time;

    public float simulationTimeFactor = 1F;
    public float timeUntilDayEnds = 300F;

    private void Awake()
    {
        instance = this;
    }

    // Setup the clock
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * simulationTimeFactor;
        
        //move clock time
        float minutes = Mathf.Floor((time / 60));
        float seconds = time % 60;

        if (time >= timeUntilDayEnds)
        {
            //day ends
        }
    }
    
    
}
