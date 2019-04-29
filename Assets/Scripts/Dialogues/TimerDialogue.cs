using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn;

public class TimerDialogue : MonoBehaviour
{
    
    public float currCountdownValue;
    [NonSerialized]
    public bool lastStrawD = false;
    public GameObject variableStorage;

    public void Update()
    {
        if (currCountdownValue >= 9)
        {
            var varStore = variableStorage.GetComponent<VariableStorage>();
            var valueToSet = new Yarn.Value(true);
            varStore.SetValue("$lastStraw", valueToSet);
            lastStrawD = true;
        }
    }

    public void OnEnable()
    {
      StartCoroutine(StartCountdown());
    }
    public void OnDisable()
    {
        currCountdownValue = 0;
    }

    public IEnumerator StartCountdown(float countdownValue = 0)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue < 10)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue++;
        }
    }
}
