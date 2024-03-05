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
    public RECIPE recipe;
    public RECIPE oldRecipe;

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
    public int canFixHp;
    public bool canFix;

    private void Awake()
    {
        //manager = GameObject.Find("GameManager").GetComponent<AIManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        recipe.recipe = null;
        oldRecipe.recipe = null;
    }

    void Start()
    {
        bt = new BehaviourTreeManager<INode>();
        bt.SetRoot(new SelectorNode
        (
            // 3�� - Dev

            // ������ ���� + ����
            new SequenceNode
            (
                // �����ǰ� ���ٸ�
                new ConditionNode(NullRecipe),
                // �����Ǹ� �����Ѵ�
                new RecipeNode(this),
                // �����ǰ� �ִٸ� (�����ǰ� �����ƴٸ�)
                new InverterNode(new ConditionNode(NullRecipe)),
                // ������ �α� ���
                new LogNode("������"),
                // ��� ���·� ����
                new ChangeStateNode(this, AIStateType.Ingredient)
            ),

                new AnimationNode(this),
            // ������ ���� �� �̵�
            new SequenceNode
            (
                // �������� ���ٸ� (���� ������ �Ͼ�ٸ�)
                new ConditionNode(NullDestination),
                // �������� ����
                new DestinationNode(this),
                // �̵�
                new InverterNode(new RangeNode(this)),
                new LogNode("�̵�"), 
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
                // ��ȣ�ۿ�
                    new WaitNode(1.5f),
                new InteractionNode(this),
                // �� ���� �ƴ϶�� (������ ȹ��)
                new InverterNode(new ConditionNode(HandNull)),
                //new WaitNode(1.5f),
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
                    //new WaitNode(1.5f),
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
                    //new WaitNode(1.5f),
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
                    // ��ȣ�ۿ�
                        new WaitNode(1.5f),
                    new InteractionNode(this),
                    //new WaitNode(1.5f),
                    // ���� �α� ���
                    new LogNode("����"),
                    // �� ���̶�� (������ ����)
                    new ConditionNode(HandNull),
                    // ����
                    new SelectorNode
                    (
                        // �ϳ� �־��� ��
                        new SequenceNode
                        (
                            new InverterNode(new ConditionNode(pick)),
                            new ActionNode(NextStep)
                            //new WaitNode(1f)
                        ),
                        // �� �� �־��� ��
                        new SequenceNode
                        (
                            new ConditionNode(pick),
                            new ActionNode(NextStep),
                            new ActionNode(EndRecovery)
                        )
                    )
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
                    //new WaitNode(1f),
                    // ��ȣ�ۿ�
                        new WaitNode(1.5f),
                    new InteractionNode(this),
                    //new WaitNode(1.5f),
                    // �� ���� �ƴ϶�� (������ ȹ��)
                    new InverterNode(new ConditionNode(HandNull)),
                    // ���� �α� ���
                    new LogNode("ȹ��"),
                    // ���� or (���� or ���)
                    new SelectorNode
                    (
                        // ����
                        new SequenceNode
                        (
                            // ���� ����̶�� (���� �������� �˻�)
                            new ConditionNode(Recovery),
                            // �ٵ� ���� �� ������� �� ��;;
                            new InverterNode(new ConditionNode(CheckTrash)),
                            new SelectorNode
                            (
                                // ���� ����
                                new SequenceNode
                                (
                                    new ConditionNode(canShelf),
                                    new LogNode("�ϴ� ���ݿ� �￩��"),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                ),
                                // ���� �ֱ�
                                new SequenceNode
                                (
                                    new ConditionNode(canMerge),
                                    new InverterNode(new ConditionNode(HandNull)),
                                        new WaitNode(1.5f),         
                                    new InteractionNode(this),
                                    //new WaitNode(1.5f), 
                                    new LogNode("�ϴ� �ٽ� �־��"),
                                    new ActionNode(NextRecipe),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                )
                            )
                        ),
                        // ����-��� ���� �˻�
                        new SelectorNode
                        (
                            new InverterNode(new ConditionNode(Recovery)),
                                new LogNode("111111111111111111111"),
                            // ����
                            new SequenceNode
                            (
                                // �����Ⱑ �ƴ϶��
                                new InverterNode(new ConditionNode(CheckTrash)),
                                // ���� �α� ���
                                new LogNode("������"),
                                new ActionNode(ClearState),
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
                                new ActionNode(ClearState),
                                // ��� ���·� ����
                                new ChangeStateNode(this, AIStateType.Trash)
                            )
                        )
                    )
                )
            ),

            // ����
            new SequenceNode
            (
                // ���� ������ �´ٸ�
                new CheckStateNode(this, AIStateType.Shelf),
                // �Ÿ��� �ȴٸ�,
                new RangeNode(this),
                new SelectorNode
                (
                    // ȸ��
                    new SequenceNode
                    (
                        new ConditionNode(canMerge),
                        new ConditionNode(HandNull),
                            new WaitNode(1.5f),         
                        new InteractionNode(this),
                        new LogNode(" ���� - ȸ��"),
                        new InverterNode(new ConditionNode(HandNull)),
                        new ChangeStateNode(this, AIStateType.Merge),
                        new ActionNode(ClearState),
                        new LogNode("ȸ�� ����")
                    ),
                    // ����
                    new SequenceNode
                    (
                        new ConditionNode(canShelf),
                        new InverterNode(new ConditionNode(HandNull)),
                            new WaitNode(1.5f),         
                        new InteractionNode(this),
                        new LogNode(" ���� - ����"),
                        new ConditionNode(HandNull),
                        new ActionNode(NextStep),
                        new ActionNode(NextRecipe)
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
                    new WaitNode(1.5f),           
                new InteractionNode(this),
                //new WaitNode(1.5f),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ���� �α� ���
                new LogNode(" ����"),
                new ActionNode(test),
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
                    new WaitNode(1.5f),           
                new InteractionNode(this),
                //new WaitNode(1.5f),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ���� �α� ���
                new LogNode(" ���"),
                // ������ �ʱ�ȭ
                new ActionNode(ClearRecipe)
            )
        ));
    }

    void Update()
    {
        bt.Update();
        stateTxt = stateType.ToString();

        if (HP.Instance.Gage.value > canFixHp)
            canFix = true;
        else
            canFix = false;
    }

    void test()
    {
        HP.Instance.SetValue(false);
    }

    bool NullRecipe()
    {
        if (recipe.recipe == null)
            return true;
        return false;
    }

    void ClearRecipe()
    {
        recipe.recipe = null;
        recipe.index = 0;
        isComplete = false;
        destination = null; 
    }

    void NextRecipe()
    {
        oldRecipe.recipe = null;
        oldRecipe.index = 0;
        stateType = AIStateType.Ingredient;
        isComplete = false;
        destination = null;
    }

    void NextStep()
    {
        if (isRecovery && oldRecipe.recipe != null && oldRecipe.index < 2)
        {
            if (oldRecipe.index < 2)
                oldRecipe.index++;

            if (oldRecipe.index == 2)
                isComplete = true;
        }
        else
        {
            if (recipe.index < 2)
                recipe.index++;

            if (recipe.index == 2)
                isComplete = true;
        }

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

    bool Recovery()
    {
        return isRecovery;
    }

    void EndRecovery()
    {
        isRecovery = false; 
    }

    bool canShelf()
    {
        return recipe.index == 0;
    }

    bool canMerge()
    {
        return recipe.index == 1;
    }

    bool pick()
    {
        return recipe.index == 2;
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
