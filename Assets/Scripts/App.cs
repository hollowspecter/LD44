using System;
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

    public class PreGame : State
    {
        public PreGame(App representation) : base(representation) { }
        public override void Update(){
            base.Update();
            var r = representation;
            if(Input.GetKeyDown(KeyCode.Space)){
                r.stateMachine.SetState(r.inGame);
            }
            if(Input.GetKeyDown(KeyCode.Escape)){
                Application.Quit();
            }
        }
    }    
    public class InGame : State
    {
        public InGame(App representation) : base(representation) { }
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
}
