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
    //public RecipeListSO oldRecipe;
    //public RecipeListSO recipe;
    //public int oldRecipeIdx;
    //public int recipeIdx;
    public List<RECIPE01> recipes = new List<RECIPE01>();
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
                //new ConditionNode(NullRecipe),
                // �����Ǹ� �����Ѵ�
                //new ActionNode(GiveRecipe),
                new RecipeNode(this),
                // �����ǰ� �ִٸ� (�����ǰ� �����ƴٸ�)
                //new InverterNode(new ConditionNode(NullRecipe)),
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
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new DestinationNode(this)
                //),
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
                    new ActionNode(NextStep)
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
                        //// ȸ�� - test .ver (recovery�� ���...)
                        //new SequenceNode
                        //(
                        //    // �ϴ� ȸ�� �װ� ����� �� �� ����...
                        //    new ConditionNode(Recovery),
                        //    // �ٵ� ���� �� ������� �� ��;;
                        //    new InverterNode(new ConditionNode(CheckTrash)),
                        //    new SelectorNode
                        //    (
                        //        // ����
                        //        new SequenceNode
                        //        (
                        //            new ConditionNode(Recovery1),
                        //            new LogNode("�ϴ� ���ݿ� �￩��"),
                        //            new ChangeStateNode(this, AIStateType.Shelf)
                        //        ),
                        //        // ������ �ƴϰ�... ȸ���̶� ... .��.��.��
                        //        new SequenceNode
                        //        (
                        //            new ConditionNode(Recovery2),
                        //            new InteractionNode(this)
                        //        )
                        //    )
                        //),
                        new SequenceNode(
                            new InverterNode(new ConditionNode(Recovery)),
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
                )
            ),

            //// ����
            //new SequenceNode
            //(
            //    // ���ݿ� �η� ���� �Ͱ� ȸ�� �Ϸ� ���� ���� �����ؾ���.)
            //    // ���� ������ �´ٸ�
            //    new CheckStateNode(this, AIStateType.Shelf),
            //    // �Ÿ��� �ȴٸ�,
            //    new RangeNode(this),
            //    new SelectorNode
            //    (
            //        // ȸ��
            //        new SequenceNode
            //        (
            //            new ConditionNode(Recovery2),
            //            new ConditionNode(HandNull),
            //            new InteractionNode(this),
            //            new LogNode(" ���� - ȸ��"),
            //            new InverterNode(new ConditionNode(HandNull)),
            //            new ChangeStateNode(this, AIStateType.Merge)
            //        ),
            //        // ����
            //        new SequenceNode
            //        (   
            //            new ConditionNode(Recovery1),
            //            new InverterNode(new ConditionNode(HandNull)),
            //            new InteractionNode(this),
            //            new LogNode(" ���� - ����"),
            //            new ConditionNode(HandNull),
            //            new ActionNode(NextRecipe)
            //        )
            //    )
            //    // �������� ��� �ִٸ�
            //    // ��ȣ�ۿ�
            //    // ��ȣ�ۿ� ����
            //    // ���� �α� ���
            //    // ������ �ʱ�ȭ
            //),

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
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new ActionNode(NextRecipe)
                //)
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
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new ActionNode(NextRecipe)
                //)
            )
        )) ;
    }

    void Update()
    {
        bt.Update();
        stateTxt = stateType.ToString();
    }

    //bool NullRecipe()
    //{
    //    if (recipe != null)
    //        return false;
    //    return true;
    //}

    //void ClearRecipe()
    //{
    //    recipe = null;
    //    isComplete = false;
    //}
    
    void ClearRecipe()
    {
        recipes[recipeIdx] = null;
        isComplete = false;
    }

    void ResetRecipe()
    {
        recipeIdx = 0;
        destination = null;
    }

    //void NextRecipe()
    //{
    //    if (oldRecipeIdx < 2)
    //        oldRecipeIdx++;

    //    if (oldRecipeIdx == 2)
    //    {
    //        oldRecipe = null;
    //        oldRecipeIdx = 0;
    //        isRecovery = false;
    //    }

    //    //if (isRecovery == false)
    //        stateType = AIStateType.Ingredient;
    //    recipeIdx = 0;

    //    destination = null;
    //}

    void NextStep()
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

    bool Recovery()
    {
        return isRecovery;
    }

    //bool Recovery1()
    //{
    //    return oldRecipeIdx == 0;
    //}

    //bool Recovery2()
    //{
    //    return oldRecipeIdx == 1;
    //}

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

    //bool Test()
    //{
    //    if (oldRecipeIdx == 1)
    //}

    bool HandNull()
    {
        return hand == null ? true : false;
    }
}
