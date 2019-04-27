﻿using System;
using System.Collections;
using System.Collections.Generic;
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
        public TellerMachine tellerMachine;
        public CustomerManager customerManager;
        public InGame(App representation) : base(representation) { }
        public override void Enter(){
            base.Enter();
            foreach(var c in customerManager.cGen){
                tellerMachine.accounts.Add(c.account.accountNumber,c.account);
            }
        }
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Escape)){
                r.stateMachine.SetState(r.preGame);
            }
        }
    }

    public PreGame preGame;
    public InGame inGame;
    private void Awake(){
        preGame.Init(this);
        inGame.Init(this);
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
