using System.Collections.Generic;
using UnityEngine;

public class AiPushdownStateMachine
{
    Stack<AiState> stateStack = new Stack<AiState>();
    Dictionary<string, AiState> states = new Dictionary<string, AiState>();

    public AiState CurrentState { get { return (stateStack.Count > 0) ? stateStack.Peek() : null; } }
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
        string str = "";

        var array = stateStack.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            str += array[i].Name;
            if (i < array.Length - 1) str += "\n";
        }

        return str;
    }

    public void PushState<T>()
    {
        PushState(typeof(T).Name);
    }
    public void PushState(string name)
    {
        if (!states.ContainsKey(name))
        {
            Debug.LogError($"State Machine already contains state {name}");
            return;
        }
        CurrentState?.OnExit();

        var nextState = states[name];

        //CurrentState.OnExit();
        stateStack.Push(nextState);
        CurrentState.OnEnter();

    }

    public void PopState()
    {
        if (stateStack.Count == 0) return;

        //call exit on current
        CurrentState.OnExit();
        //pop current state
        stateStack.Pop();
        //enter new state
        CurrentState?.OnEnter();
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

       while(stateStack.Count > 0)
        {
            stateStack.Pop();
        }

        var newState = states[name];
        //push new state
        stateStack.Push(newState);
        //enter new state (current state)
        CurrentState.OnEnter();
    }
}
