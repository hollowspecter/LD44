
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TellerMachine : MonoBehaviour, IDraggableReceiver {
    public Dispensary dispensary;
    public Dictionary<string,Account> accounts = new Dictionary<string, Account>();
    public string firstNameField { get; set;}
    public string lastNameField { get; set;}
    public string accountNumberField { get; set;}
    public InputField firstNameText;
    public InputField lastNameText;
    public InputField accountNumberText;
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
        if(accountNumberField == null || accountNumberField == ""){ return;}
        if(accounts.ContainsKey(accountNumberField)){
            firstNameText.text = accounts[accountNumberField].firstName;
            lastNameText.text = accounts[accountNumberField].lastName;
            balanceText.text = "Balance: "+accounts[accountNumberField].balance.ToString("C");
        }else{
            firstNameText.text = "";
            lastNameText.text = "";
            balanceText.text = "Balance: $0.00";
        }
    }

    private IAccountReceiver receiver;
    public void RegisterAccountReceiver(IAccountReceiver receiver){        
        if(this.receiver != null){
            throw new Exception("Only one receiver per TellerMachine");
        }
        this.receiver = receiver;
    }

    public static System.Random random = new System.Random();
    public void CreateAccount(){
        if(receiver != null && receiver.hasAccount){
            return;
        } 
        if(firstNameField == ""){
            Error();
            return;
        }
        if(lastNameField == ""){
            Error();
            return;            
        }
        string accountNumber = random.Next().ToString("000000");
        while(accounts.ContainsKey(accountNumber)){
            accountNumber = random.Next().ToString("000000");
        }
        accounts[accountNumber] = new Account();
        accounts[accountNumber].accountNumber = accountNumber;
        accounts[accountNumber].firstName = firstNameField;
        accounts[accountNumber].lastName = lastNameField;
        accounts[accountNumber].balance = 0f;
        accountNumberText.text = accountNumber;
        if(receiver != null){
            receiver.OnAccountCreated(accounts[accountNumber]);
        }        
        print(accountNumber);
        Success();
    }

    public void Withdraw(){
        Withdraw(amountField);
    }

    private void Withdraw(float amount){
        if(accounts.ContainsKey(accountNumberField)){
            if(accounts[accountNumberField].balance >= amount){
                accounts[accountNumberField].balance -= amount;
                Success();
                return;
            }
        }
        Error();
    }

    public void Deposit(){
        if(accounts.ContainsKey(accountNumberField)){
            accounts[accountNumberField].balance += amountField;
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
                    if(accounts.ContainsKey(accountNumberField)){
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