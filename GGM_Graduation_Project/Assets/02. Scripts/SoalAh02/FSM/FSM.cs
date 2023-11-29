using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> where T : IState
{
    private Dictionary<string, T> states = new Dictionary<string, T>();
    private T currentState;

    public void AddState(string stateName, T state)
    {
        states[stateName] = state;
    }

    public void ChangeState(string stateName)
    {
        if (states.ContainsKey(stateName))
        {
            currentState?.Exit();
            currentState = states[stateName];
            currentState.Enter();
        }
        else
        {
            Debug.Log($"State not found: {stateName}");
        }
    }

    public void Update()
    {
        currentState?.Update();
    }
}