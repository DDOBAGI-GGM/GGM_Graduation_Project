using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

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
    public int recipeIdx;
    public RecipeListSO AArecipe; // �׽�Ʈ��
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
        //bt.SetRoot(new SequenceNode
        //(
        //    new SelectorNode
        //    (
        //        new DestinationNode(this),  
        //        new MoveNode(this, 3f),
        //        //new WaitNode(1f),
        //        new SequenceNode
        //        (
        //            // ��� ����
        //            new ConditionNode(HandNull),
        //            new LogNode("111"),
        //            //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Ingredient),
        //            //new ConditionNode<GameObject>(HandNull, hand, null),
        //            //new RangeNode(this),
        //            new InteractionNode(this),
        //            new ActionNode(StateChange)
        //        ),
        //        //new WaitNode(1f),
        //        new DestinationNode(this),
        //        new MoveNode(this, 3f),
        //        new SequenceNode
        //        (
        //            //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Processing),
        //            new ConditionNode(HandLevel_Raw),
        //            //new InverterNode(new ConditionNode<GameObject>(HandNull, hand, null)),
        //            //new RepeaterNode(new InteractionNode(this), false, true, 4), // �����ͷ� interaction 3�� �ݺ��ϴ� �κ�(����)
        //            //new RangeNode(this),
        //            new InteractionNode(this),
        //            new LogNode("222"),
        //            new ActionNode(StateChange)
        //        ),
        //        //new WaitNode(1f),
        //        new DestinationNode(this),
        //        new MoveNode(this, 3f),
        //        new SequenceNode
        //        (
        //            //new ConditionNode<AIStateType>(HandLevel, state, AIStateType.Merge),
        //            new WaitNode(2f),
        //            new ConditionNode(HandLevel_Pro),
        //            //new RangeNode(this),
        //            new InteractionNode(this),
        //            //new ActionNode(HandClear),
        //            new SequenceNode
        //            (
        //                // ������ �ϼ��̶��?
        //                //new InverterNode(new ConditionNode(HandNull)),
        //                new ConditionNode(aa),
        //                new LogNode("���"),
        //                new ActionNode(HandClear),
        //                new InteractionNode(this),
        //                new SequenceNode
        //                (
        //                    //new ConditionNode(bb),
        //                    new ActionNode(tt),
        //                    new DestinationNode(this),
        //                    new MoveNode(this, 3f),
        //                    new InteractionNode(this)
        //                )
        //            )
        //            //new ActionNode(t)
        //            //new ActionNode(StateChange),
        //        )
        //    //new LogNode("1ȸ�� ����"),
        //    )
        //));

        bt.SetRoot(new SelectorNode
        (
            // ������ ����
            new SequenceNode
            (
                new ConditionNode(NullRecipe), // �����ǰ� ���ٸ�
                new ActionNode(GiveRecipe), // �����Ǹ� �����Ѵ�
                new InverterNode(new ConditionNode(NullRecipe)), // �����ǰ� �ִٸ�
                new ActionNode(ResetRecipe) // ������ ���� �������� �̵��Ѵ�
            ),

            // ������ �� é�Ͱ� �����ٸ�
            new SequenceNode
            (
                new ConditionNode(HandNull), // ���� ����ִٸ�
                new ActionNode(CheckStep), // ���� ������ �ܰ迡 �´� �������� �����Ѵ�. (destination ���...)
                new MoveNode(this, 3f) // �������� �����ߴٸ�
            ),

            // ��� ����
            new SequenceNode
            (
                new SelectorNode
                (
                    new SequenceNode
                    (
                        new ConditionNode(HandNull),
                        new ActionNode(HandClear),
                        new LogNode("���� ���")
                    ),
                    new SequenceNode
                    (
                        new ConditionNode(HandNull),
                        new InteractionNode(this),
                        new InverterNode(new ConditionNode(HandNull)),
                        new ActionNode(ChangePro)
                    )
                )
            ),

            new DestinationNode(this),
            new SequenceNode
            (
                // ��� ����
                new ConditionNode(HandNull),
                new LogNode("111"),
                new InteractionNode(this),
                new ActionNode(StateChange)
            ),
            new DestinationNode(this),
            new MoveNode(this, 3f),
            new SequenceNode
            (
                new ConditionNode(HandLevel_Raw),
                new InteractionNode(this),
                new LogNode("222"),
                new ActionNode(StateChange)
            ),
            new DestinationNode(this),
            new MoveNode(this, 3f),
            new SequenceNode
            (
                new WaitNode(2f),
                new ConditionNode(HandLevel_Pro),
                new InteractionNode(this),
                new SequenceNode
                (
                    // ������ �ϼ��̶��?
                    new ConditionNode(aa),
                    new LogNode("���"),
                    new ActionNode(HandClear),
                    new InteractionNode(this),
                    new SequenceNode
                    (
                        new ActionNode(tt),
                        new DestinationNode(this),
                        new MoveNode(this, 3f),
                        new InteractionNode(this)
                    )
                )
            )
        ));
    }

    void Update()
    {
        bt.Update();
    }


    // 22
    bool NullRecipe()
    {
        if (recipe != null)
            return false;
        return true;
    }

    void GiveRecipe()
    {
        recipe = AArecipe;
    }

    void ResetRecipe()
    {
        recipeIdx = 0;
    }

    void CheckStep()
    {
        string temp = ExtractName(recipe.recipe[recipeIdx]);
        switch (temp)
        {
            case "completion":
            {
                GameObject target = null;
                temp = ExtractName(temp);
                foreach (ITEM str in manager.objects[0].obj)
                {
                    if (str.name == temp)
                        target = str.item;
                }
                if (target == null)
                    Debug.LogError("�������� ������ �� ����");
                break;
            }
            case "Pot":
            {
                foreach (ITEM str in manager.objects[0].obj)
                {
                    if (str.name == temp)
                        target = str.item;
                }
                if (target == null)
                    Debug.LogError("�������� ������ �� ����");
                break;
            }
            case "Floor":
            case "Object":
            case "Enemy":
                Debug.LogError("�̰��� ��");
                break;
            default:
                Debug.LogError("�̷����� ���µ�... ����$��3����! �ФѤ�");
                break;
            }
        }

    string ExtractName(string itemName)
    {
        string pattern = @"-_";
        Match match = Regex.Match(itemName, pattern);
        return match.Value;
    }

    void ChangePro()
    {
        destination = manager.objects[1].obj[0].item;
    }
    //

    void t()
    {
        state = -1;
    }
    void tt()
    {
        state = 2;
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
            state = -1;
        }
    }

    bool HandNull()
    {
        return hand == null ? true : false;
    }

    void HandClear()
    {
        hand = null;
    }

    bool aa()
    {
        MergeIngredient item = destination.GetComponent<MergeIngredient>();
        if (item.one && item.two)
        {
            Debug.Log("���Դ�```");
            return true;
        }
        return false;
    }

    bool bb()
    {
        if (hand.name == "Trash")
            return true;
        return false;
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
