using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine
{
    Dictionary<string, AiState> states = new Dictionary<string, AiState>();

    public AiState CurrentState { get; private set; }
    public void AddState(AiState state)
    {
        if(states.ContainsKey(state.Name))
        {
            Debug.LogError($"State Machine already contains state {state.Name}");
            return;
        }

        states[state.Name] = state;
    }

    public string GetString()
    {
        return (CurrentState != null) ? CurrentState.Name : "No State";
    }

    public void SetState<T>()
    {
        SetState(typeof(T).Name);
    }

    public void Update()
    {
        CurrentState?.OnUpdate();
    }

    public void SetState(string name)
    {
        if (!states.ContainsKey(name))
        {
            Debug.LogError($"State Machine already contains state {name}");
            return;
        }

        var nextState = states[name];

        //don't allow reentry to state
        if (nextState == null || nextState == CurrentState) {
            return;
        }

        //exit current state
        CurrentState?.OnExit();
        CurrentState = nextState;
        //enter next state
        CurrentState?.OnEnter();
        //update new current state
       // CurrentState?.OnUpdate();
        
    }
}
