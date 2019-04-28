using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class App : MonoBehaviour {
    public StateMachine<App> stateMachine;
    [Serializable]
    public class State : StateMachine<App>.State
    {
        public GameObject[] stateObjects;
        public State(App representation) : base(representation) { }

        public override void Init(App r){
            base.Init(r);
            foreach(var g in stateObjects){
                g.SetActive(false);
            }
        }

        public override void Enter(){
            base.Enter();
            print("Enter " + this);

            foreach(var g in stateObjects){
                g.SetActive(true);
            }
        }
        public override void Exit(){
            base.Exit();
            print("Exit " + this);

            foreach(var g in stateObjects){
                g.SetActive(false);
            }
        }
    }

    [Serializable]
    public class PreGame : State
    {
        public PreGame(App representation) : base(representation) { }
        public override void Enter(){
            base.Enter();
            var r = representation;
            r.day = 0;
        }
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Space)){
                r.stateMachine.SetState(r.inGame);
            }
        }
    }    
    [Serializable]
    public class InGame : State
    {
        public float existingBalance = 0f;
        public InGame(App representation) : base(representation) { }
        public override void Enter(){
            base.Enter();
            var r = representation;
            existingBalance = 0f;
            if(r.customerProvider.existingAccounts.Length > 0){

            }
            foreach(Account account in r.customerProvider.existingAccounts){
                r.tellerMachine.accounts.Add(account.accountNumber,account);
                existingBalance += account.balance;
            }
            // sample difficulty formula
            r.customerProvider.CreateQueue(r.day*2+3);
            customer = NextCustomer();
        }
        public ICustomer customer;
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Escape)){
                r.stateMachine.SetState(r.preGame);
            }
          
            if(customer != null){ 
                if(Input.GetKeyDown(KeyCode.Space)){
                    if(!customer.fulfilledNeed){
                            // TODO actually trigger by dialogue
                            customer.OnDialogueTrigger("deposit");
                    }else{
                        customer = NextCustomer();
                    }
                }  
            }else{                                       
                if(r.customerProvider.allCustomersLeft){                        
                    r.stateMachine.SetState(r.endOfDay);
                }                
            }
        }

        private ICustomer NextCustomer(){
            var r = representation;
            var customer = r.customerProvider.ProceedCustomer();
            if(customer != null){      
                customer.OnArrivedAtDesk(r.customerDesk);
                return customer;     
            }   
            return null;
        }
    }
    [Serializable]
    public class EndOfDay : State
    {
        public EndOfDay(App representation) : base(representation) { }
        public override void Enter(){
            var r = representation;
            int fulfilledCustomers = 0;
            foreach(ICustomer c in r.customerProvider.customers){
                if(c.fulfilledNeed){
                    fulfilledCustomers++;
                }
            }

            float moneyMeantForDeposit = r.customerDesk.dispensed;
            float targetBalance = r.inGame.existingBalance+moneyMeantForDeposit;
            float actualBalance = r.tellerMachine.balance;
            float difference = actualBalance - targetBalance;
            float lostMoney = Mathf.Min(0f,difference);
            float extraMoney = Mathf.Max(0f,difference);

            print("difference" + difference);

            r.day++;
        }
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Space)){
                r.stateMachine.SetState(r.inGame);
            }
        }
    }

    [Serializable]
    public class Score {
        public float happinessScore = 0f;
        public int numberOfCustomers = 0;
    }
    public Score score;
    public int day = 0;
    public TellerMachine tellerMachine;
    public SampleCustomerManager customerManager;
    public ICustomerProvider customerProvider => customerManager;
    public Dispensary customerDesk;

    public PreGame preGame;
    public InGame inGame;
    public EndOfDay endOfDay;
    private void Awake(){
        preGame.Init(this);
        inGame.Init(this);
        endOfDay.Init(this);
        stateMachine = new StateMachine<App>();
    }
    private void Start()
    {
        stateMachine.SetState(preGame);
    }
    private void Update()
    {
        stateMachine.Update();
    }

    public void StartGame(){
        stateMachine.SetState(inGame);
    }
    public void EndGame(){
        Application.Quit();
    }
}
