
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TellerMachine : MonoBehaviour, IDraggableReceiver {
    public Dispensary dispensary;
    public Dictionary<string,float> accounts = new Dictionary<string, float>();
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
    private void OnEnable(){
        dispensary.RegisterReceiver(this);
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

    private void Success(){
        messageText.text = "SUCCESS";
    }
    private void Error(){
        messageText.text = "ERROR";
    }
    public void Dispense(){        
        float dispensed = dispensary.DispenseChange(amountField);
        balance -= dispensed;
    }

    public bool OnReceivedDraggable(Draggable drag)
    {
        switch(drag.type){
                case Draggable.Type.Bill:
                case Draggable.Type.Coin:
                case Draggable.Type.GoldBar:
                case Draggable.Type.SilverBar:{
                    if(accounts.ContainsKey(accountName)){
                        balance += drag.value;
                        Success();
                        return true;
                    }else{
                        Error();
                        return false;
                    }
                }
                case Draggable.Type.Generic:{
                    Error();
                    return false;
                }
                default: {
                    throw new Exception("Unhandled Draggable!");
                }
            }
    }
}