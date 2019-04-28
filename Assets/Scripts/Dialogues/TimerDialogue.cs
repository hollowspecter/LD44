using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn;

public class TimerDialogue : MonoBehaviour
{
    public float currCountdownValue;
    public GameObject variableStorage;

    public void Update()
    {
        //if (currCountdownValue == 0)

        var varStore = variableStorage.GetComponent<VariableStorage>();
        var valueToSet = new Yarn.Value(true);
        varStore.SetValue("$lastStraw", valueToSet);
        //Debug.Log(valueToSet);

        var newName = new Yarn.Value("GRANDMA");
        varStore.SetValue("$newName", newName);
        //Debug.Log(newName);
    }

    public void StartCount()
    {
      StartCoroutine(StartCountdown());
    }
    
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
           
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }
}
