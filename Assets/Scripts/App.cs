using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
    public static App instance;
    public float time;
    public GameObject soundManagerPrefab;

    public float simulationTimeFactor = 1F;
    public float timeUntilDayEnds = 800F;

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
    public class InGame : State
    {
        public InGame(App representation) : base(representation) { }
        public override void Enter(){
            base.Enter();
        }
        public override void Update(){
            base.Update();
        }
    }
    [Serializable]
    public class EndOfDay : State
    {
        public EndOfDay(App representation) : base(representation) { }

        public override void Enter(){
            base.Enter();
            App r = representation;
            // TODO show score etc...
            print("Total happiness: " + r.score.happiness);
        }
    }
    [Serializable]
    public class Score {
        public float happiness = 0f;
        public float moneyDifference = 0f;
    }
    public Score score = new Score ();

    public InGame inGame;
    public EndOfDay endOfDay;
    private void Awake(){
        if(instance != null){
            throw new Exception("Only one App allowed!");
        }
        instance = this;
        inGame.Init(this);
        stateMachine = new StateMachine<App>();
        if (SoundManager.Instance == null)
        {
            Instantiate ( soundManagerPrefab );
        }
    }
    private void Start()
    {
        stateMachine.SetState(inGame);
    }
    private void Update()
    {
        time += Time.deltaTime * simulationTimeFactor;
        if(time > timeUntilDayEnds)
        {stateMachine.SetState(endOfDay);}
        stateMachine.Update();
    }

    public void StartGame(){
        stateMachine.SetState(inGame);
    }
    public void EndGame(){
        Application.Quit();
    }
}
