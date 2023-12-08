using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T stateMachineClass;

    public State() { }

    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public abstract void OnUpdate(float deltaTime);
    public virtual void OnEnd() { }

    internal void SetMachineWithClass(StateMachine<T> stateMachine, T stateMachineClass)
    {
        this.stateMachine = stateMachine;
        this.stateMachineClass = stateMachineClass;
        OnAwake();
    }
}
