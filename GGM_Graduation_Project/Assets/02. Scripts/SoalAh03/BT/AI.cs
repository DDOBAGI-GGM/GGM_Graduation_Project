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
    public AIManager manager;
    BehaviourTreeManager<INode> bt;

    public NavMeshAgent agent;

    public int state;
    public AIStateType stateType;
    //public AIStateType state;
    public GameObject hand;
    public Transform handPos;
    //public GameObject target;
    public GameObject destination; // �׽�Ʈ��
    public RecipeListSO recipe; // �׽�Ʈ��
    public int recipeIdx;
    public RecipeListSO AArecipe; // �׽�Ʈ��
    public IObject test;
    //public Transform destination;

    public string stateTxt;
    public bool isComplete = false; // �����ϸ� manager recipe �����ϴ� ���� bool ���� ������ �������� �����ϵ��� �ٲٱ�!


    private void Awake()
    {
        manager = GameObject.Find("AIMgr").GetComponent<AIManager>();
        agent = GetComponent<NavMeshAgent>();
        //hand.transform.SetParent(handPos.transform);
    }

    void Start()
    {
        bt = new BehaviourTreeManager<INode>();
        // 1��
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
            // 3��
            // ������ ���� (�����ǰ� ������� ��)
            new SequenceNode
            (
                new ConditionNode(NullRecipe), // �����ǰ� ���ٸ�
                new LogNode("������ ����!!!"),
                new ActionNode(GiveRecipe), // �����Ǹ� �����Ѵ�
                new InverterNode(new ConditionNode(NullRecipe)), // �����ǰ� �ִٸ�
                new ActionNode(ResetRecipe), // ������ ���� �������� �̵��Ѵ�
                //new ChangeStateNode(this, AIStateType.Ingredient)
                new ActionNode(ChangeIngredient),
                new LogNode(stateType.ToString() + " ������ ���� �� ����")
                ),

            // ������ ���� (1. ���� ���� Ȯ��, 2. �� ���� Ȯ��)
            new SequenceNode
            (
                new ConditionNode(NullDestination),
                new ActionNode(Destination)
                //new MoveNode(this, 3f),
            ),

            //// ������ ���� (1. ���� ���� Ȯ��, 2. �� ���� Ȯ��)
            //new SequenceNode
            //(
            //    new LogNode("dd")
            //    //new InverterNode(new ConditionNode(NullDestination)),
            //    //new LogNode(destination.name + " - ������ ����") 
            //),

            // ��� ����
            new SequenceNode
            (
                //new ActionNode(Destination),
                new InverterNode(new ConditionNode(NullDestination)),
                new MoveNode(this, 3f),
                // ��� ���� ������ �´���?
                //new CheckStateNode(this, AIStateType.Ingredient),
                new ConditionNode(StepIngredient),
                new LogNode(stateType.ToString() + " ��� ����"),
                    //new ConditionNode(StepIngredient),
                // ���� �������
                new ConditionNode(HandNull),
                // ��ȣ�ۿ�
                new InteractionNode(this),
                // ����� �տ� ���Դ���?
                new InverterNode(new ConditionNode(HandNull)),
                // ���� �ܰ� ������ NextStep
                new ActionNode(NextStep),
                //new ChangeStateNode(this, AIStateType.Processing)
                new ActionNode(ChangePro)
            ),

            // ���� üũ 
            new SequenceNode
            (
                new ConditionNode(StepProcessing),
                new InverterNode(new ConditionNode(HandNull)),
                // ������ �ʿ����� �ʴٸ�
                new InverterNode(new ConditionNode(NeedProcessing)),
                // ���� �ܰ� ������ NextStep
                new ActionNode(NextStep),
                //new ChangeStateNode(this, AIStateType.Merge)
                new ActionNode(ChangeMer)
            ),

            // ����
            new SequenceNode
            (
                //new ActionNode(Destination),
                new InverterNode(new ConditionNode(NullDestination)),
                new MoveNode(this, 3f),
                // ���� ������ �´���?
                //new CheckStateNode(this, AIStateType.Processing),
                new ConditionNode(StepProcessing),
                new LogNode(stateType.ToString() + " ������"),
                    //new ConditionNode(StepProcessing), // �� ����
                // ����� �ƴ���
                new InverterNode(new ConditionNode(HandNull)),
                // ��ȣ�ۿ� (��ȣ�ۿ� �����ߴٴ� �����Ͽ� Success ��ȯ)
                new InteractionNode(this), // �̰� ������ success�� ��ȯ�ؼ� ���峵�� ��... �갡 �����̴�
                new LogNode("���� ����..."),
                // ���� �ܰ� ������ NextStep
                new ActionNode(NextStep),
                //new ChangeStateNode(this, AIStateType.Merge)
                new ActionNode(ChangeMer)
            ),

            // ����
            new SequenceNode
            (
                //new ActionNode(Destination),
                new InverterNode(new ConditionNode(NullDestination)),
                new MoveNode(this, 3f),
                // ���� ������ �´���?
                //new CheckStateNode(this, AIStateType.Merge),
                new ConditionNode(StepMerge),
                //new ConditionNode(StepProcessing), // �� ����
                // �� ���� �ƴ���
                new InverterNode(new ConditionNode(HandNull)),
                // ��ȣ�ۿ� (��ȣ�ۿ� �����ߴٴ� �����Ͽ� Success ��ȯ)
                new InteractionNode(this),
                // �������� �ϼ� �ƴ��� (��ȣ�ۿ� �� null�� �ƴ϶�� �������� ���� ��)
                new LogNode(stateType.ToString() + " ������"),
                //new InverterNode(new ConditionNode(HandNull)),
                /////// ���� ������ ���� or (���� or ��������) (�� �� �ϳ��� �����) - ���� �� �� �ϰ� ���� ���� ���� 
                // ���� �ϼ��� �ƴ϶��
                new SelectorNode
                (
                    // ���� ������ ����
                    new SequenceNode
                    (
                        // �� �� �̶��
                        new ConditionNode(HandNull),
                        new LogNode("���� 1�ܰ�"),
                        // ���� ������ ������ �����Ѵ� (state�� ingredient ����)
                        new ActionNode(NextRecipe)
                    )
                    //// ���� or ��������
                    //new SequenceNode
                    //(
                    //    // �� ���� �ƴ϶�� (������ ȹ��)
                    //    new InverterNode(new ConditionNode(HandNull)),
                    //    new SelectorNode
                    //    (
                    //        // ����
                    //        new SequenceNode
                    //        (
                    //            // �����Ⱑ �ƴ϶��
                    //            new InverterNode(new ConditionNode(CheckTrash)),
                    //            new LogNode("������ ȹ��"),
                    //            // ���� State�� attack���� �������ְ�
                    //            //new ChangeStateNode(this, AIStateType.Attack)
                    //            new ActionNode(ChangeAttackt)
                    //        ),
                    //        // ��������
                    //        new SequenceNode
                    //        (
                    //            // ��������
                    //            new InverterNode(new ConditionNode(CheckTrash)),
                    //            new LogNode("������ ȹ��"),
                    //            // ���� State�� trash�� �������ְ�
                    //            //new ChangeStateNode(this, AIStateType.Trash)
                    //            new ActionNode(ChangeAttackt)
                    //        )
                    //    ),
                    //    // ������ ����
                    //    new ActionNode(Destination),
                    //    // �̵�
                    //    new MoveNode(this, 3f),
                    //    // ��ȣ�ۿ�
                    //    new LogNode(stateType.ToString() + " ���� or ���"),
                    //    new InteractionNode (this),
                    //    // ��ȣ�ۿ� ����
                    //    new ConditionNode(HandNull),
                    //    // ������ ����
                    //    new ActionNode(ClearRecipe)
                    //) // - ���� or �������� �� ��...
                ) // - ���� �ϼ��� �ƴ� ��,...
            ),

            // ���� ȸ��
            new SequenceNode
            (
                // ���� ȸ�� ������ �´���?
                new ConditionNode(MergeComplete),
                // �� ��?
                new ConditionNode(HandNull),
                new InverterNode(new ConditionNode(NullDestination)),
                new MoveNode(this, 3f),
                    new LogNode("���d??"),
                //new ConditionNode(StepMerge),
                // ������ �ϼ��ΰ�?
                new InteractionNode(this),
                // �������� �ϼ� �ƴ��� (��ȣ�ۿ� �� null�� �ƴ϶�� �������� ���� ��)

                // ���� or ��������
                new SequenceNode
                (
                    // �� ���� �ƴ϶�� (������ ȹ��)
                    new InverterNode(new ConditionNode(HandNull)),
                    new LogNode("ȹ��"),
                    new ActionNode(ChangeNone),
                    new SelectorNode
                    (
                        // ����
                        new SequenceNode
                        (
                            // �����Ⱑ �ƴ϶��
                            new InverterNode(new ConditionNode(CheckTrash)),
                            new LogNode("������ ȹ��"),
                            // ���� State�� attack���� �������ְ�
                            //new ChangeStateNode(this, AIStateType.Attack)
                            new ActionNode(ChangeAttack)
                        ),
                        // ��������
                        new SequenceNode
                        (
                            // ��������
                            new ConditionNode(CheckTrash),
                            new LogNode("������ ȹ��"),
                            // ���� State�� trash�� �������ְ�
                            //new ChangeStateNode(this, AIStateType.Trash)
                            new ActionNode(ChangeTrash)
                        )
                    )
                    //new LogNode("���� �� ����?..."), // - ���� �� �� ������ ���� ����;
                    //// ������ ����
                    //new ActionNode(Destination),
                    //// �̵�
                    //new MoveNode(this, 3f),
                    //// ��ȣ�ۿ�
                    //new LogNode(stateType.ToString() + " ���� or ���"),
                    //new InteractionNode(this),
                    //// ��ȣ�ۿ� ����
                    //new ConditionNode(HandNull),
                    //// ������ ����
                    //new ActionNode(ClearRecipe)
                ) // - ���� or �������� �� ��...
            ),

            // ����
            new SequenceNode
            (
                // ���� ������ �´ٸ�
                new ConditionNode(StepAttack),
                new InverterNode(new ConditionNode(HandNull)),
                // ������ ����
                new ActionNode(Destination),
                // �̵�
                new MoveNode(this, 3f),
                // ��ȣ�ۿ�
                new LogNode(stateType.ToString() + " ����"),
                new InteractionNode(this),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ������ ����
                new ActionNode(ClearRecipe)
            ),

            // ���
            new SequenceNode
            (
                // ��� ������ �´ٸ�
                new ConditionNode(StepTrash),
                new InverterNode(new ConditionNode(HandNull)),
                // ������ ����
                new ActionNode(Destination),
                // �̵�
                new MoveNode(this, 3f),
                // ��ȣ�ۿ�
                new LogNode(stateType.ToString() + " ���"),
                new InteractionNode(this),
                // ��ȣ�ۿ� ����
                new ConditionNode(HandNull),
                // ������ ����
                new ActionNode(ClearRecipe)
            )

        // 3-1��
        //// ������ �� é�Ͱ� �����ٸ� (���� ������� ��)
        //new SequenceNode
        //(
        //    new ConditionNode(HandNull), // ���� ����ִٸ�
        //    new ActionNode(CheckStep), // ���� ������ �ܰ迡 �´� �������� �����Ѵ�. (destination ���...)
        //    new MoveNode(this, 3f) // �������� �����ߴٸ� 
        //),
        //// ��� ����
        //new SequenceNode
        //(
        //    new SelectorNode
        //    (
        //        new SequenceNode
        //        (
        //            new ConditionNode(HandNull),
        //            new ActionNode(HandClear),
        //            new LogNode("���� ���")
        //        ),
        //        new SequenceNode
        //        (
        //            new ConditionNode(HandNull),
        //            new InteractionNode(this),
        //            new InverterNode(new ConditionNode(HandNull)),
        //            new ActionNode(ChangePro)
        //        )
        //    )
        //),

        // 2��
        //new DestinationNode(this),
        //new SequenceNode
        //(
        //    // ��� ����
        //    new ConditionNode(HandNull),
        //    new LogNode("111"),
        //    new InteractionNode(this),
        //    new ActionNode(StateChange)
        //),
        //new DestinationNode(this),
        //new MoveNode(this, 3f),
        //new SequenceNode
        //(
        //    new ConditionNode(HandLevel_Raw),
        //    new InteractionNode(this),
        //    new LogNode("222"),
        //    new ActionNode(StateChange)
        //),
        //new DestinationNode(this),
        //new MoveNode(this, 3f),
        //new SequenceNode
        //(
        //    new WaitNode(2f),
        //    new ConditionNode(HandLevel_Pro),
        //    new InteractionNode(this),
        //    new SequenceNode
        //    (
        //        // ������ �ϼ��̶��?
        //        new ConditionNode(aa),
        //        new LogNode("���"),
        //        new ActionNode(HandClear),
        //        new InteractionNode(this),
        //        new SequenceNode
        //        (
        //            new ActionNode(tt),
        //            new DestinationNode(this),
        //            new MoveNode(this, 3f),
        //            new InteractionNode(this)
        //        )
        //    )
        //)
        )) ;
    }

    void Update()
    {
        bt.Update();

        stateTxt = stateType.ToString();
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

    void ClearRecipe()
    {
        recipe = null;
        isComplete = false;
    }

    void ResetRecipe()
    {
        //stateType = AIStateType.Ingredient;
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

    void CheckStep() // ActionNode�� ���� �θ��� �ʰ� �ٸ� �Լ��� ���� �θ��� �� == �̰� RecipeNode�� ����!
    {
        //Debug.Log(recipe.recipe[recipeIdx] + " ���� ������");
        string temp = ExtractName(recipe.recipe[recipeIdx]);
        string prefix = null;
        GameObject target = null;
        switch (temp)
        {
            case "completion":
                {
                    prefix = ExtractPrefix(recipe.recipe[recipeIdx]);
                    foreach (ITEM str in manager.objects[0].obj)
                    {
                        if (str.name == prefix)
                            target = str.item;
                    }
                    if (target == null)
                        Debug.LogError("�������� ������ �� ����");
                    break;
                }
            case "Pot":
                {
                    Debug.Log("����Ʈ �尡��");
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

        destination = target;
    }

    string ExtractPrefix(string itemName)
    {
        return itemName.Split('-')[0];
    }

    string ExtractName(string itemName)
    {
        string pattern = @"-";
        Match match = Regex.Match(itemName, pattern);
        //    Debug.Log(itemName);    
        //if (itemName.Split('-')[1] == null)
        //    Debug.Log("  ddfdf");    
        return itemName.Split('-')[1];
    }

    //void ChangePro()
    //{
    //    destination = manager.objects[1].obj[0].item;
    //}

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

   void NextStep()
   {
        destination = null;
        //stateType++;
        //if (((int)stateType) > (int)AIStateType.Shelf)
        //    Debug.LogError("���� �����÷�");
   }

    //// ��� ���� �Ű������� Aistatetype �����ؼ� ��... �Ʒ��� �ӽ�
    bool StepIngredient()
    {
        if (stateType == AIStateType.Ingredient)
            return true;
        return false;
    }

    bool StepProcessing()
    {
        if (stateType == AIStateType.Processing)
            return true;
        return false;
    }

    bool StepMerge()
    {
        if (stateType == AIStateType.Merge)
            return true;
        return false;
    }

    bool StepAttack()
    {
        if (stateType == AIStateType.Attack)
            return true;
        return false;
    }

    bool StepTrash()
    {
        if (stateType == AIStateType.Trash)
            return true;
        return false;
    }

    void ChangeIngredient()
    {
        stateType = AIStateType.Ingredient;
    }

    void ChangePro()
    {
        stateType = AIStateType.Processing;
    }
    void ChangeMer()
    {
        stateType = AIStateType.Merge;
    }
    void ChangeAttack()
    {
        stateType = AIStateType.Attack;
    }

    void ChangeTrash()
    {
        stateType = AIStateType.Trash;
    }

    void ChangeNone()
    {
        stateType = AIStateType.None;
    }


    void Destination()
    {
        switch (stateType)
        {
            case AIStateType.Ingredient:
            {
                // ���� ������ ������ �´� ��� ����
                //Debug.Log("���");
                CheckStep();
                break;
            }
            case AIStateType.Processing:
            {
                // 1. ���� �Ǵ� ��������� / 2. �Ÿ���
                destination = manager.objects[1].obj[0].item; // �ӽ�
                break;
            }
            case AIStateType.Merge:
            {
                // 1. ���� �Ǵ� ��������� / 2. �Ÿ���
                destination = manager.objects[2].obj[0].item; // �ӽ�
                break;
            }
            case AIStateType.Attack:
            {
                destination = manager.objects[3].obj[0].item;
                break;
            }
            case AIStateType.Trash:
                {
                    destination = manager.objects[4].obj[0].item;
                    break;
                }
        }
    }

    bool CheckTrash()
    {
        Debug.Log("�˻� ������");
        if (hand.name == "Trash")
            return true;
        return false;
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
