using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFSM : MonoBehaviour
{
    protected StateMachine<AIFSM> fsmManager;

    private void Start()
    {
        // ���°����� ���� �� ���� �߰�
        //fsmManager = new StateMachine<AIFSM>(this, new StateIdle());
        //fsmManager.AddStateList(new StateMove());
        //fsmManager.AddStateList(new StateAtk());
    }

    // 
    public LayerMask targetLayerMask;
    public float eyeSight;
    public Transform target;
    public Transform SearchEnemy()
    {
        target = null;

        Collider[] findTargets
            = Physics.OverlapSphere(transform.position, eyeSight, targetLayerMask);

        if (findTargets.Length > 0)
        {
            target = findTargets[0].transform;
        }

        return target;
    }

    public float atkRange;
    public bool getFlagAtk
    {
        get
        {
            if (!target)
            {
                return false;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            return (distance <= atkRange);
        }
    }

    private void Update()
    {
        fsmManager.Update(Time.deltaTime);
    }
}