
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TellerMachine : MonoBehaviour, IDraggableReceiver {
    public Dispensary dispensary;
    public Dictionary<string,Account> accounts = new Dictionary<string, Account>();
    public string firstNameField { get; set;}
    public string lastNameField { get; set;}
    public string accountNumberField { get; set;}
    public TMP_InputField firstNameText;
    public TMP_InputField lastNameText;
    public TMP_InputField accountNumberText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI balanceText;
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
        accounts.Clear();
        receiver = null;
    }
    private void Update(){
        Account a;
        if(TryGetAccount(accountNumberField,out a)){
            firstNameText.text = a.firstName;
            lastNameText.text = a.lastName;
            balanceText.text = "Balance: "+a.balance.ToString("C");
        }else {
            balanceText.text = "Balance: $0.00";
        }
    }

    private ICustomer receiver;
    public void RegisterAccountReceiver(ICustomer receiver){        
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
        Account a;
        if(TryGetAccount(accountNumberField,out a)){
            if(a.balance >= amount){
                a.balance -= amount;
                Success();
                return;
            }
        }
        Error();
    }

    public void Deposit(){
        Account a;
        if(TryGetAccount(accountNumberField,out a)){
            a.balance += amountField;
            Success();
            return;
        }
        Error();
    }
    private bool TryGetAccount(string accountNumber, out Account account){
        if(accountNumber == null || accountNumber == ""){ 
            account = null;
            return false; 
        }
        if(accounts.TryGetValue(accountNumber,out account)){
            return true;
        }
        return false;
    }

    private void Success(){
        messageText.text = "SUCCESS";
    }
    private void Error(){
        messageText.text = "ERROR";
        SoundManager.Instance.PlaySfxAsOneShot ( "UIAlert" );
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
                balance += drag.value;
                Success();
                return true;
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