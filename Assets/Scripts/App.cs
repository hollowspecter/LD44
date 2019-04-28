using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
    public static App instance;

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
        public InGame(App representation) : base(representation) { }
        public override void Enter(){
            base.Enter();
        }
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Escape)){
                r.stateMachine.SetState(r.preGame);
            }
        }
    }
    [Serializable]
    public class EndOfDay : State
    {
        public EndOfDay(App representation) : base(representation) { }

        public override void Enter(){
            base.Enter();
            var r = representation;
            // TODO show score etc...
            print("Total happiness: " + r.score.happiness);
        }
    }
    [Serializable]
    public class Score {
        public float happiness = 0f;
        public float lostMoney = 0f;
        public float extraMoney = 0f;
    }
    public Score score;

    public PreGame preGame;
    public InGame inGame;
    private void Awake(){
        if(instance != null){
            throw new Exception("Only one App allowed!");
        }
        instance = this;
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
