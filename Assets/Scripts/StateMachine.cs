using System;

public class StateMachine<T> {
    private State currentState = null;
    public class State {
        public T representation;
        public State(T representation) {
            this.representation = representation;
        }
        public void Init(T representation){
            this.representation = representation;
        }

        public virtual void Enter()
        { }

        public virtual void Exit()
        { }

        public virtual void Update()
        { }
    }

    public StateMachine(State initialState) {
        SetState(initialState);
    }
    public StateMachine() { }

    public void SetState(State newState) {
        if(currentState == newState){return;}
        if(currentState != null){
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void Update() {
        if(currentState != null) {
            currentState.Update();
        }
    }
}