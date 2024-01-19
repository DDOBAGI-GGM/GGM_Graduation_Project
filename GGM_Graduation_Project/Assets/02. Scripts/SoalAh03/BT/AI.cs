using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public AIManager manager;
    BehaviourTreeManager<INode> bt;

    public NavMeshAgent agent;

    [SerializeField] private Transform handPos;

    public AIStateType state;
    public GameObject hand;
    public GameObject target;
    public GameObject destination; // �׽�Ʈ��
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
        bt.SetRoot(new SelectorNode
        (
            new SequenceNode
            (
                new DestinationNode(this, state),
                new MoveNode(this, destination, 2f),
                // range... target ����...
                new InteractionNode(this, destination),
                new LogNode("1ȸ�� ����"),
                new WaitNode(5f)
            )
        ));
    }

    void Update()
    {
        bt.Update();
    }
}
