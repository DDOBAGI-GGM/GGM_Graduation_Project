using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIFSM : MonoBehaviour
{
    protected StateMachine<AIFSM> fsmManager;
    protected BehaviourTreeManager<INode> btManager;

    NavMeshAgent navAgent;

    public LayerMask targetLayerMask;
    public Transform target;
    public float eyeSight;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //상태관리자 생성 및 상태 추가
        fsmManager = new StateMachine<AIFSM>(this, new StopState());
        fsmManager.AddStateList(new MoveState());

        btManager = new BehaviourTreeManager<INode>();
        btManager.SetRoot(new SelectorNode
            (new SequenceNode(/*조리법*/
                new ConditionNode(haveRecipe),
                new ActionNode(ChoiceRecipe)),
            new SequenceNode(/**/
                new ConditionNode(haveHand),
                new ActionNode(useTable))));
    }

    //public Transform SearchEnemy()
    //{
    //    target = null;

    //    Collider[] findTargets
    //        = Physics.OverlapSphere(transform.position, eyeSight, targetLayerMask);

    //    if (findTargets.Length > 0)
    //        target = findTargets[0].transform;

    //    return target;
    //}

    //public float atkRange;
    //public bool getFlagAtk
    //{
    //    get
    //    {
    //        if (!target)
    //        {
    //            return false;
    //        }

    //        float distance = Vector3.Distance(transform.position, target.position);
    //        return (distance <= atkRange);
    //    }
    //}

    private void Update()
    {
        fsmManager.Update(Time.deltaTime);
        btManager.Update();
    }
    bool haveRecipe()
    {
        return true;
    }

    void ChoiceRecipe()
    {

    }

    bool haveHand()
    {
        return true;
    }

    void useTable()
    {

    }
}