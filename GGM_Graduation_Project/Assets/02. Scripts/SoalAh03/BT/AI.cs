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
                //new ActionNode(ChangeIngredient)
                new ChangeStateNode(this, AIStateType.Ingredient)
                ),

            // ������ ���� �� �̵�
            new SequenceNode
            (
                // �������� ���ٸ� (���� ������ �Ͼ�ٸ�)
                new ConditionNode(NullDestination),
                // �������� ����
                //new ActionNode(Destination),
                new DestinationNode(this),
                // �̵�
                new MoveNode(this, 3f)
            ),

            // ��� ����
            new SequenceNode
            (
                // ��� ������ �´ٸ�
                //new ConditionNode(StepIngredient),
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
                //new ActionNode(ChangePro)
                new ChangeStateNode(this, AIStateType.Processing)
            ),

            // ���� (���� �˻�, ����)
            new SelectorNode
            (
                // ���� �˻�
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    //new ConditionNode(StepProcessing),
                    new CheckStateNode(this, AIStateType.Processing),
                    // �������� ��� �ִٸ�
                    new InverterNode(new ConditionNode(HandNull)),
                    // ������ �ʿ����� �ʴٸ�
                    new InverterNode(new ConditionNode(NeedProcessing)),
                    // ���� �ʱ�ȭ
                    new ActionNode(ClearState),
                    // ���� ���·� ����
                    //new ActionNode(ChangeMer)
                    new ChangeStateNode(this, AIStateType.Merge)
                ),

                // ����
                new SequenceNode
                (
                    // ���� ������ �´ٸ�
                    //new ConditionNode(StepProcessing),
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
                    //new ActionNode(ChangeMer)
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
                    //new ConditionNode(StepMerge),
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
                    //new ConditionNode(StepMerge),
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
                            //new ActionNode(ChangeAttack)
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
                            //new ActionNode(ChangeTrash)
                            new ChangeStateNode(this, AIStateType.Trash)
                        )
                    )
                )
            ),

            // ����
            new SequenceNode
            (
                // ���� ������ �´ٸ�
                //new ConditionNode(StepAttack),
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
                //new ConditionNode(StepTrash),
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

   void ClearState()
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
