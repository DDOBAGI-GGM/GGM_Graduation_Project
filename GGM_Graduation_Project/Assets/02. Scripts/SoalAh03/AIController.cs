using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public AI brain;

    protected StateMachine<AIController> fsmManager;
    protected BehaviourTreeManager<INode> btManager;

    public NavMeshAgent navAgent;

    //public LayerMask targetLayerMask;
    public Transform target;
    //public float eyeSight;

    public bool tArride = false;

    private void Awake()
    {
        brain = GetComponent<AI>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //���°����� ���� �� ���� �߰�
        fsmManager = new StateMachine<AIController>(this, new StopState());
        fsmManager.AddStateList(new MoveState());

        btManager = new BehaviourTreeManager<INode>();
        btManager.SetRoot(new SelectorNode
            (new SequenceNode(/*������ ����*/
                new ConditionNode(haveRecipe),
                new ActionNode(ChoiceRecipe)),
            new SequenceNode(/*��� ���� �غ�*/
                new ConditionNode(haveHand),
                new MoveNode(move),
                new ActionNode(useTable)),
            /*��� ����*/
            new MoveNode(move),
            new ActionNode(ChoiceIngredent),
            new SequenceNode(/*���� �ϱ�*/
                new ConditionNode(needMake),
                new ActionNode(make)),
            /*������ �ֱ�*/
            new ActionNode(putItem)));
    }

    private void Update()
    {
        fsmManager.Update(Time.deltaTime);
        btManager.Update();
    }

    bool move()
    {
        tArride = false;
        target = brain.objects[0].transform;
        fsmManager.ChangeState<MoveState>();

        if (tArride == true)
            return true;
        return false;
    }

    // ������ ������ �ִ���
    bool haveRecipe()
    {
        Debug.Log("���չ� �˻�");
        if (brain.recipe != null)
            return false;
        return true;
    }

    // ������ ����
    void ChoiceRecipe()
    {
        Debug.Log("���չ� ����");
        brain.recipe = brain.recipes[0];
    }

    // �� ������
    bool haveHand()
    {
        Debug.Log("�� �� �˻�");
        if (brain.hand != null)
            return true;
        return false;   
    }

    // ���� ���
    void useTable()
    {
        Debug.LogError("���� ���");
        //tArride = false;
        //// ���� ã��
        //target = brain.objects[0].transform;
        //// �������� ��
        //fsmManager.ChangeState<MoveState>();

        //if (tArride)
        //{
        //    // ����!
        //    fsmManager.ChangeState<StopState>();
        //    if (brain.hand == null)
        //        Debug.Log("���� ����µ� ���� ����� ����");
        //    else
        //    {
        //        //���ݻ��! -> ���� �����ϱ� �ϴ� ���� ���ܹ���
        //        Destroy(brain.hand.GetComponentInChildren<GameObject>());
        //    }
        //}
    }

    // ��� ����
    void ChoiceIngredent()
    {
        if (tArride)
        {
            Debug.Log("��� ����");
            IObject objectToPickup = target.GetComponent<IObject>();           // ������Ʈ ��������
            if (objectToPickup != null)
            {
                //Debug.Log(objectToPickup);
                GameObject pickUpItem = objectToPickup.Interaction();
                if (pickUpItem != null)
                {
                    pickUpItem.transform.position = brain.handPos.position;        // ������Ʈ �� ��ġ�� �̵�
                    pickUpItem.transform.parent = brain.handPos;       // ���� �ڽ����� ����
                    brain.hand = pickUpItem;     // �տ� ��� ����!

                    return;
                }
            }
        }
    }

    // ���� �ʿ��Ѱ�
    bool needMake()
    {
        Debug.Log("���� ���� �˻�");
        return true;
    }

    void make()
    {
        Debug.Log("���� �ϱ�");

    }

    void putItem()
    {
        Debug.Log("������ �ֱ�");

    }
}