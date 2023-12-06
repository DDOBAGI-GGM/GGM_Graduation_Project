using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachine<T>
{
    private T stateMachineClass;

    private State<T> nowState;
    public State<T> getNowState => nowState;

    private State<T> beforeState;
    public State<T> getBeforeState => beforeState;

    private float stateDurationTime = .0f;
    public float getStateDurationTime => stateDurationTime;

    private Dictionary<System.Type, State<T>> stateLists
        = new Dictionary<System.Type, State<T>>();

    public StateMachine(T stateMachineClass, State<T> initState)
    {
        this.stateMachineClass = stateMachineClass;

        AddStateList(initState);
        nowState = initState;
        nowState.OnStart();
    }

    public void AddStateList(State<T> state)
    {
        state.SetMachineWithClass(this, stateMachineClass);
        stateLists[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        stateDurationTime += deltaTime;
        nowState.OnUpdate(deltaTime);
    }

    public Q ChangeState<Q>() where Q : State<T>
    {
        var newType = typeof(Q);

        if (nowState.GetType() == newType)
            return nowState as Q;

        if (nowState != null)
            nowState.OnEnd();

        beforeState = nowState;
        nowState = stateLists[newType];

        nowState.OnStart();
        stateDurationTime = .0f;

        return nowState as Q;
    }
}