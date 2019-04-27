
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TellerMachine : MonoBehaviour {
    public Dictionary<string,float> accounts = new Dictionary<string, float>();
    public TriggerProxy analyzerTrigger;
    public Transform outlet;
    public Draggable[] changePrefabs;
    public float dispenseForce = 1f;
    public float dispenseDelay = 0.3f;
    public string firstNameField { get; set;}
    public string lastNameField { get; set;}
    public string accountName {
        get { return firstNameField + lastNameField; }
    }
    public Text messageText;
    public Text balanceText;
    private static CultureInfo ci = new CultureInfo("en-US");
    public string amountText {get; set;}
    public float amountField { 
        get {
            float f;
            float.TryParse(amountText,NumberStyles.Any,ci,out f);
            return f;
        }
    }
    public float balance = 0f;
    private Queue<Draggable> dispenseQueue = new Queue<Draggable>();
    private Coroutine dispenseRoutine;
    private void OnEnable(){
        SortChange();
        dispenseRoutine = StartCoroutine(DispenseRoutine());
        analyzerTrigger.stay = (c)=>OnTriggerStay(c);
    }
    private void OnDisable(){
        StopAllCoroutines();
    }
    private void Update(){
        if(accounts.ContainsKey(accountName)){
            balanceText.text = "Balance: "+accounts[accountName].ToString("C");
        }else{
            balanceText.text = "Balance: $0.00";
        }
    }

    private void SortChange(){
        float[] values = new float[changePrefabs.Length];
        for(int i = 0; i < values.Length; i++){
            values[i] = changePrefabs[i].value;
        }
        Array.Sort(values,changePrefabs);
    }

    private IEnumerator DispenseRoutine(){
        while(enabled){
            if(dispenseQueue.Count > 0){
                var drag = dispenseQueue.Dequeue();
                drag.gameObject.SetActive(true);
                drag.transform.position = outlet.position;
                drag.rigid.position = outlet.position;
                drag.rigid.velocity = Vector3.zero;
                drag.rigid.angularVelocity = Vector3.zero;
                drag.rigid.AddForce(outlet.forward*dispenseForce, ForceMode.VelocityChange);
            }
            yield return new WaitForSeconds(dispenseDelay);
        }
    }

    private void OnTriggerStay(Collider collider){
        Draggable drag = collider.GetComponent<Draggable>();
        if(drag){
            switch(drag.type){
                case Draggable.Type.Bill:
                case Draggable.Type.Coin:
                case Draggable.Type.GoldBar:
                case Draggable.Type.SilverBar:{
                    if(accounts.ContainsKey(accountName)){
                        Keep(drag);
                        Success();
                    }else{
                        DispenseQueued(drag);
                        Error();
                    }
                    break;
                }
                case Draggable.Type.Generic:{
                    DispenseQueued(drag);
                    Error();
                    break;
                }
            }
        }
    }

    public void CreateAccount(){
        if(accounts.ContainsKey(accountName)){
            Error();
            return;
        }
        accounts[accountName] = 0f;
        Success();
    }

    public void Withdraw(){
        Withdraw(amountField);
    }

    private void Withdraw(float amount){
        if(accounts.ContainsKey(accountName)){
            if(accounts[accountName] >= amount){
                accounts[accountName] -= amount;
                Success();
                return;
            }
        }
        Error();
    }

    public void Deposit(){
        if(accounts.ContainsKey(accountName)){
            accounts[accountName] += amountField;
            Success();
            return;
        }
        Error();
    }

    private void Keep(Draggable drag){
        balance += drag.value;  
        Destroy(drag.gameObject);
    }

    public void Dispense(){        
        Dispense(amountField);
    }
    private void Dispense(float amount){
        float dispensed = 0;
        if(changePrefabs.Length > 0){
            for(int i = changePrefabs.Length-1; i >= 0;i--){
                var prefab = changePrefabs[i];
                while(prefab.value <= amount){
                    Draggable drag = Instantiate<Draggable>(prefab);
                    dispensed += drag.value;
                    amount -= drag.value;
                    DispenseQueued(drag);
                }
            }
        }
        balance -= dispensed;
    }

    private void DispenseQueued(Draggable drag){    
        drag.gameObject.SetActive(false);    
        dispenseQueue.Enqueue(drag);
    }

    private void Success(){
        messageText.text = "SUCCESS";
    }
    private void Error(){
        messageText.text = "ERROR";
    }
}