using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public AIManager manager;
    BehaviourTreeManager<INode> bt;

    public NavMeshAgent agent;

    public int state;
    //public AIStateType state;
    public GameObject hand;
    public Transform handPos;
    public GameObject target;
    public GameObject destination; // �׽�Ʈ��
    public RecipeListSO recipe; // �׽�Ʈ��
    public IObject test;
    //public Transform destination;


    private void Awake()
    {
        manager = GameObject.Find("AIMgr").GetComponent<AIManager>();
        agent = GetComponent<NavMeshAgent>();
        //hand.transform.SetParent(handPos.transform);
    }

    void Start()
    {
        bt = new BehaviourTreeManager<INode>();
        bt.SetRoot(new SequenceNode
        (
            new SelectorNode
            (
                new DestinationNode(this),
                new MoveNode(this, 2f),
                new WaitNode(1f),
                new SequenceNode
                (
                    // ��� ����
                    new ConditionNode(HandNull),
                    //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Ingredient),
                    //new ConditionNode<GameObject>(HandNull, hand, null),
                    new InteractionNode(this),
                    new ActionNode(StateChange)
                ),
                new WaitNode(1f),
                new DestinationNode(this),
                new MoveNode(this, 2f),
                new SequenceNode
                (
                    //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Processing),
                    new ConditionNode(HandLevel_Raw),
                    new LogNode("111"),
                    //new InverterNode(new ConditionNode<GameObject>(HandNull, hand, null)),
                    //new RepeaterNode(new InteractionNode(this), false, true, 4), // �����ͷ� interaction 3�� �ݺ��ϴ� �κ�(����)
                    new InteractionNode(this),
                    new LogNode("222"),
                    new ActionNode(StateChange)
                ),
                new WaitNode(1f),
                new DestinationNode(this),
                new MoveNode(this, 2f),
                new SequenceNode
                (
                    //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Merge),
                    new ConditionNode(HandLevel_Pro),
                    new InteractionNode(this),
                    new ActionNode(StateChange),
                    new LogNode("��")
                )
            //new LogNode("1ȸ�� ����"),
            )
        ));
    }

    void Update()
    {
        bt.Update();
    }

    void StateChange()
    {
        //state += 1;

        if (hand != null)
        {
            TwoIngredient two = hand.GetComponent<TwoIngredient>();
            if (two == null)
            {
                ThreeIngredient three = hand.GetComponent<ThreeIngredient>();
                state = ((int)three.type);
            }
            else
            {
                state = ((int)two.type);
            }
        }
        else
        {
            state = 0;
        }
    }

    bool HandNull()
    {
        return hand == null ? true : false;
    }

    bool HandLevel_Raw()
    {
        return state ==  0 ? true : false;  
    }

    bool HandLevel_Pro()
    {
        return state == 1 ? true : false;
    }

    bool HandLevel_Mer()
    {
        return state == 2 ? true : false;
    }
}
