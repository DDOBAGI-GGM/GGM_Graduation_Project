using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Action : MonoBehaviour
{
    protected FSM_Brain _brain;
    [SerializeField] protected float speed;

    protected virtual void Awake()
    {
        _brain = GetComponentInParent<FSM_Brain>();
    }

    public abstract void Action();
}
