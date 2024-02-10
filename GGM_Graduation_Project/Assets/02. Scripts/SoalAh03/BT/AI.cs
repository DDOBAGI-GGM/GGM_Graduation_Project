using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    [Header("Attribute")]
    public AIManager manager;
    public BehaviourTreeManager<INode> bt;

    [Header("Component")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Recipe")]
    public RecipeListSO recipe;
    public int recipeIdx;

    [Header("Destination")]
    public GameObject destination;

    [Header("State")]
    public AIStateType stateType;
    public bool isComplete = false;
    public bool isRecovery = false;

    [Header("Hand")]
    public GameObject hand;
    public Transform handPos;

    [Header("Test")]
    public string stateTxt;
    // �����ϸ� manager recipe �����ϴ� ���� bool ���� ������ �������� �����ϵ��� �ٲٱ�!
    // -> ����. ���� ����Ϸ���?..


    private void Awake()
    {
        manager = GameObject.Find("AIMgr").GetComponent<AIManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        bt = new BehaviourTreeManager<INode>();
        bt.SetRoot(new SelectorNode
        (
            // 3�� - Dev
            // ������ ����
            new SequenceNode
            (
                // �����ǰ� ���ٸ�
                new ConditionNode(NullRecipe),
                // �����Ǹ� �����Ѵ�
                //new ActionNode(GiveRecipe),
                new RecipeNode(this),
                // �����ǰ� �ִٸ� (�����ǰ� �����ƴٸ�)
                new InverterNode(new ConditionNode(NullRecipe)),
                // ������ �α� ���
                new LogNode("������"),
                // ������ �ʱ�ȭ
                new ActionNode(ResetRecipe),
                // ��� ���·� ����
                new ChangeStateNode(this, AIStateType.Ingredient)
                ),

            // ������ ���� �� �̵�
            new SequenceNode
            (
                // �������� ���ٸ� (���� ������ �Ͼ�ٸ�)
                new ConditionNode(NullDestination),
                // �������� ����
                new DestinationNode(this),
                // �̵�
                new MoveNode(this, 3f)
            ),

            // ��� ����
            new SequenceNode
            (
                // ��� ������ �´ٸ�
                new CheckStateNode(this, AIStateType.Ingredient),
                // �Ÿ��� �ȴٸ�
                new RangeNode(this),
                // �� ���̶��
                new ConditionNode(HandNull),
                new WaitNode(1f),
                // ��ȣ�ۿ�
                new InteractionNode(this),
                // �� ���� �ƴ϶�� (������ ȹ��)
                new InverterNode(new ConditionNode(HandNull)),
                // ���� �α� ���
                new LogNode("���"),
                // ���� �ʱ�ȭ
                new ActionNode(ClearState),
                // ���� ���·� ����
                new ChangeStateNode(this, AIStateType.Processing)
            ),

            // ���� (���� �˻�, ����)
            new SelectorNode
            (
                // ���� �˻�
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    new CheckStateNode(this, AIStateType.Processing),
                    // �������� ��� �ִٸ�
                    new InverterNode(new ConditionNode(HandNull)),
                    // ������ �ʿ����� �ʴٸ�
                    new InverterNode(new ConditionNode(NeedProcessing)),
                    // ���� �ʱ�ȭ
                    new ActionNode(ClearState),
                    // ���� ���·� ����
                    new ChangeStateNode(this, AIStateType.Merge)
                ),

                // ����
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    new CheckStateNode(this, AIStateType.Processing),
                    // �Ÿ��� �ȴٸ�
                    new RangeNode(this),
                    // �������� ��� �ִٸ�
                    new InverterNode(new ConditionNode(HandNull)),
                    // ��ȣ�ۿ� �α� ���
                    new LogNode("������"),
                    // ��ȣ�ۿ�
                    new InteractionNode(this),
                    // ���� �α� ���
                    new LogNode("����"),
                    // ���� �ʱ�ȭ
                    new ActionNode(ClearState),
                    // ���� ���·� ����
                    new ChangeStateNode(this, AIStateType.Merge)
                )
            ),

            // ���� (�ֱ�, ���)
            new SelectorNode
            (
                // �ֱ�
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    new CheckStateNode(this, AIStateType.Merge),
                    // ȸ�� �ܰ谡 �ƴ϶��
                    new InverterNode(new ConditionNode(MergeComplete)),
                    // �Ÿ��� �ȴٸ�
                    new RangeNode(this),
                    // �������� ��� �ִٸ�
                    new InverterNode(new ConditionNode(HandNull)),
                    new WaitNode(1f),
                    // ��ȣ�ۿ�
                    new InteractionNode(this),
                    // ���� �α� ���
                    new LogNode("����"),
                    // �� ���̶�� (������ ����)
                    new ConditionNode(HandNull),
                    // ������ ���� �ܰ�
                    new ActionNode(NextRecipe)
                ),

                // ȸ��
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    new CheckStateNode(this, AIStateType.Merge),
                    // ȸ�� �ܰ谡 �´ٸ�
                    new ConditionNode(MergeComplete),
                    // �Ÿ��� �ȴٸ�
                    new RangeNode(this),
                    // �� ���̶��
                    new ConditionNode(HandNull),
                    new WaitNode(1f),
                    // ��ȣ�ۿ�
                    new InteractionNode(this),
                    // �� ���� �ƴ϶�� (������ ȹ��)
                    new InverterNode(new ConditionNode(HandNull)),
                    // ���� �α� ���
                    new LogNode("ȹ��"),
                    // ���� �ʱ�ȭ
                    new ActionNode(ClearState),
                    // ����-��� ���� �˻�
                    new SelectorNode
                    (
                        // ����
                        new SequenceNode
                        (
                            // �����Ⱑ �ƴ϶��
                            new InverterNode(new ConditionNode(CheckTrash)),
                            // ���� �α� ���
                            new LogNode("������"),
                            // ���� ���·� ����
                            new ChangeStateNode(this, AIStateType.Attack)
                        ),
                        // ���
                        new SequenceNode
                        (
                            // ��������
                            new ConditionNode(CheckTrash),
                            // ���� �α� ���
                            new LogNode("���"),
                            // ��� ���·� ����
                            new ChangeStateNode(this, AIStateType.Trash)
                        )
                    )
                )
            ),

            // ����
            new SequenceNode
            (
                // ���� ������ �´ٸ�
                new CheckStateNode(this, AIStateType.Attack),
                // �Ÿ��� �ȴٸ�,
                new RangeNode(this),
                // �������� ��� �ִٸ�
                new InverterNode(new ConditionNode(HandNull)),
                // ��ȣ�ۿ�
                new InteractionNode(this),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ���� �α� ���
                new LogNode(" ����"),
                // ������ �ʱ�ȭ
                new ActionNode(ClearRecipe)
            ),

            // ���
            new SequenceNode
            (
                // ��� ������ �´ٸ�
                new CheckStateNode(this, AIStateType.Trash),
                // �Ÿ��� �ȴٸ�,
                new RangeNode(this),
                // �������� ��� �ִٸ�
                new InverterNode(new ConditionNode(HandNull)),
                // ��ȣ�ۿ�
                new InteractionNode(this),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ���� �α� ���
                new LogNode(" ���"),
                // ������ �ʱ�ȭ
                new ActionNode(ClearRecipe)
            )
        )) ;
    }

    void Update()
    {
        bt.Update();
        stateTxt = stateType.ToString();
    }


 
    bool NullRecipe()
    {
        if (recipe != null)
            return false;
        return true;
    }

    void ClearRecipe()
    {
        recipe = null;
        isComplete = false;
    }

    void ResetRecipe()
    {
        recipeIdx = 0;
        destination = null;
    }

    void NextRecipe()
    {
        if (recipeIdx < 2)
            recipeIdx++;

        if (recipeIdx == 2)
            isComplete = true;

        if (isComplete == false)
            stateType = AIStateType.Ingredient;
        destination = null;
    }

    bool NeedProcessing()
    {
        Ingredient ingredient = hand.GetComponent<Ingredient>();
        if (ingredient != null)
            return true;
        return false;
    }

    bool MergeComplete()
    {
        return isComplete;
    }

    bool NullDestination()
    {
        if (destination != null)
            return false;
        return true;
    }

   void ClearState()
   {
        destination = null;
   }

    bool CheckTrash()
    {
        if (hand.name == "Trash")
            return true;
        return false;
    }

    bool HandNull()
    {
        return hand == null ? true : false;
    }
}
