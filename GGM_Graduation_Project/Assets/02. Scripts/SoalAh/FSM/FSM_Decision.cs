using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Decision : MonoBehaviour
{
    protected FSM_Brain _brain;

    protected virtual void Awake()
    {
        _brain = GetComponentInParent<FSM_Brain>();
    }

    public abstract bool Decision();
}
