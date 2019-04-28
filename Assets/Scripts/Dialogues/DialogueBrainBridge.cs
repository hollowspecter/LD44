using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class DialogueBrainBridge : VariableStorageBehaviour
{
    public void Init(CustomerBrain brain){
        // TODO link all the booleans to actual customer states
        
    }

    public override void ResetToDefaults ()
    {

    }

    public override void SetValue (string variableName, Yarn.Value value)
    {
        //print(variableName);
    }
    // DEBUG 
    public bool accountCheck = false;
    public bool depositCheck = true;
    public bool withdrawCheck = true;
    public bool robCheck = true;
    public override Yarn.Value GetValue (string variableName)
    {
        var val = Yarn.Value.NULL;
        if(variableName == "$Yarn.ShuffleOptions"){return val;}
        print(variableName);
        switch(variableName){
            case "$has_account":{
                if(accountCheck){ 
                    val = new Yarn.Value(true);
                }
                accountCheck = true;
                break;
            }
            case "$needs_deposit":{
                if(depositCheck){ 
                    val = new Yarn.Value(true);
                }
                depositCheck = false;
                break;
            }
            case "$needs_withdraw":{
                if(withdrawCheck){ 
                    val = new Yarn.Value(true);
                }
                withdrawCheck = false;
                break;
            }
            case "$needs_rob":{
                if(robCheck){ 
                    val = new Yarn.Value(true);
                }
                robCheck = false;
                break;
            }
        }
        return val;
    }

    public override void Clear ()
    {

    }

    public IEnumerator RunCommand(string command){
        print("Command: " + command);
        // TODO Handle commands!
        switch(command){
            case "trigger Customer account":{
                //
                break;
            }
            case "trigger Customer withdraw":{
                //
                break;
            }
            case "trigger Customer deposit":{
                //
                break;
            }
            case "trigger Customer rob":{
                //
                break;
            }
            case "trigger Customer leave":{
                //
                break;
            }
        }
        yield return new WaitForSeconds(2f);
    }

}
