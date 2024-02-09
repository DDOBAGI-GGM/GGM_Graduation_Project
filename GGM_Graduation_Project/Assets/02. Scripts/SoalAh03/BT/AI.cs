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
    public GameObject destination; // 테스트용
    public RecipeListSO recipe; // 테스트용
    public int recipeIdx;
    public RecipeListSO AArecipe; // 테스트용
    public IObject test;
    //public Transform destination;

    public string stateTxt;
    public bool isComplete = false; // 가능하면 manager recipe 저장하는 곳에 bool 만들어서 레시피 각각에서 관리하도록 바꾸기!


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
            // 3차 - Dev
            // 레시피 선택
            new SequenceNode
            (
                // 레시피가 없다면
                new ConditionNode(NullRecipe),
                // 레시피를 지정한다
                //new ActionNode(GiveRecipe),
                new RecipeNode(this),
                // 레시피가 있다면 (레시피가 지정됐다면)
                new InverterNode(new ConditionNode(NullRecipe)),
                // 레시피 로그 출력
                new LogNode("레시피"),
                // 레시피 초기화
                new ActionNode(ResetRecipe),
                // 재료 상태로 변경
                //new ActionNode(ChangeIngredient)
                new ChangeStateNode(this, AIStateType.Ingredient)
                ),

            // 목적지 설정 및 이동
            new SequenceNode
            (
                // 목적지가 없다면 (상태 변경이 일어났다면)
                new ConditionNode(NullDestination),
                // 목적지를 설정
                //new ActionNode(Destination),
                new DestinationNode(this),
                // 이동
                new MoveNode(this, 3f)
            ),

            // 재료 선택
            new SequenceNode
            (
                // 재료 스탭이 맞다면
                //new ConditionNode(StepIngredient),
                new CheckStateNode(this, AIStateType.Ingredient),
                // 거리가 된다면
                new RangeNode(this),
                // 빈 손이라면
                new ConditionNode(HandNull),
                new WaitNode(1f),
                // 상호작용
                new InteractionNode(this),
                // 빈 손이 아니라면 (아이템 획득)
                new InverterNode(new ConditionNode(HandNull)),
                // 상태 로그 출력
                new LogNode("재료"),
                // 상태 초기화
                new ActionNode(ClearState),
                // 가공 상태로 변경
                //new ActionNode(ChangePro)
                new ChangeStateNode(this, AIStateType.Processing)
            ),

            // 가공 (가공 검사, 가공)
            new SelectorNode
            (
                // 가공 검사
                new SequenceNode
                (
                    // 가공 스탭이 맞다면
                    //new ConditionNode(StepProcessing),
                    new CheckStateNode(this, AIStateType.Processing),
                    // 아이템을 들고 있다면
                    new InverterNode(new ConditionNode(HandNull)),
                    // 가공이 필요하지 않다면
                    new InverterNode(new ConditionNode(NeedProcessing)),
                    // 상태 초기화
                    new ActionNode(ClearState),
                    // 병합 상태로 변경
                    //new ActionNode(ChangeMer)
                    new ChangeStateNode(this, AIStateType.Merge)
                ),

                // 가공
                new SequenceNode
                (
                    // 가공 스탭이 맞다면
                    //new ConditionNode(StepProcessing),
                    new CheckStateNode(this, AIStateType.Processing),
                    // 거리가 된다면
                    new RangeNode(this),
                    // 아이템을 들고 있다면
                    new InverterNode(new ConditionNode(HandNull)),
                    // 상호작용 로그 출력
                    new LogNode("가공중"),
                    // 상호작용
                    new InteractionNode(this),
                    // 상태 로그 출력
                    new LogNode("가공"),
                    // 상태 초기화
                    new ActionNode(ClearState),
                    // 병합 상태로 변경
                    //new ActionNode(ChangeMer)
                    new ChangeStateNode(this, AIStateType.Merge)
                )
            ),

            // 병합 (넣기, 얻기)
            new SelectorNode
            (
                // 넣기
                new SequenceNode
                (
                    // 병합 스탭이 맞다면
                    //new ConditionNode(StepMerge),
                    new CheckStateNode(this, AIStateType.Merge),
                    // 회수 단계가 아니라면
                    new InverterNode(new ConditionNode(MergeComplete)),
                    // 거리가 된다면
                    new RangeNode(this),
                    // 아이템을 들고 있다면
                    new InverterNode(new ConditionNode(HandNull)),
                    new WaitNode(1f),
                    // 상호작용
                    new InteractionNode(this),
                    // 상태 로그 출력
                    new LogNode("병합"),
                    // 빈 손이라면 (아이템 부착)
                    new ConditionNode(HandNull),
                    // 레시피 다음 단계
                    new ActionNode(NextRecipe)
                ),

                // 회수
                new SequenceNode
                (
                    // 병합 스탭이 맞다면
                    //new ConditionNode(StepMerge),
                    new CheckStateNode(this, AIStateType.Merge),
                    // 회수 단계가 맞다면
                    new ConditionNode(MergeComplete),
                    // 거리가 된다면
                    new RangeNode(this),
                    // 빈 손이라면
                    new ConditionNode(HandNull),
                    new WaitNode(1f),
                    // 상호작용
                    new InteractionNode(this),
                    // 빈 손이 아니라면 (아이템 획득)
                    new InverterNode(new ConditionNode(HandNull)),
                    // 상태 로그 출력
                    new LogNode("획득"),
                    // 상태 초기화
                    new ActionNode(ClearState),
                    // 공격-폐기 상태 검사
                    new SelectorNode
                    (
                        // 공격
                        new SequenceNode
                        (
                            // 쓰레기가 아니라면
                            new InverterNode(new ConditionNode(CheckTrash)),
                            // 정보 로그 출력
                            new LogNode("아이템"),
                            // 공격 상태로 변경
                            //new ActionNode(ChangeAttack)
                            new ChangeStateNode(this, AIStateType.Attack)
                        ),
                        // 폐기
                        new SequenceNode
                        (
                            // 쓰레기라면
                            new ConditionNode(CheckTrash),
                            // 정보 로그 출력
                            new LogNode("폐기"),
                            // 폐기 상태로 변경
                            //new ActionNode(ChangeTrash)
                            new ChangeStateNode(this, AIStateType.Trash)
                        )
                    )
                )
            ),

            // 공격
            new SequenceNode
            (
                // 공격 스탭이 맞다면
                //new ConditionNode(StepAttack),
                new CheckStateNode(this, AIStateType.Attack),
                // 거리가 된다면,
                new RangeNode(this),
                // 아이템을 들고 있다면
                new InverterNode(new ConditionNode(HandNull)),
                // 상호작용
                new InteractionNode(this),
                // 상호작용 성공
                new ConditionNode(HandNull),
                // 상태 로그 출력
                new LogNode(" 공격"),
                // 레시피 초기화
                new ActionNode(ClearRecipe)
            ),

            // 폐기
            new SequenceNode
            (
                // 폐기 스탭이 맞다면
                //new ConditionNode(StepTrash),
                new CheckStateNode(this, AIStateType.Trash),
                // 거리가 된다면,
                new RangeNode(this),
                // 아이템을 들고 있다면
                new InverterNode(new ConditionNode(HandNull)),
                // 상호작용
                new InteractionNode(this),
                // 상호작용 성공
                new ConditionNode(HandNull),
                // 상태 로그 출력
                new LogNode(" 폐기"),
                // 레시피 초기화
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

    void CheckStep() // ActionNode를 통해 부르지 않고 다른 함수에 의해 부르는 중 == 이걸 RecipeNode에 넣자!
    {
        //Debug.Log(recipe.recipe[recipeIdx] + " 현재 레시피");
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
                        Debug.LogError("목적지를 설정할 수 없음");
                    break;
                }
            case "Pot":
                {
                    Debug.Log("페인트 드가자");
                    foreach (ITEM str in manager.objects[0].obj)
                    {
                        if (str.name == temp)
                            target = str.item;
                    }
                    if (target == null)
                        Debug.LogError("목적지를 설정할 수 없음");
                    break;
                }
            case "Floor":
            case "Object":
            case "Enemy":
                Debug.LogError("미개발 ㅋ");
                break;
            default:
                Debug.LogError("이럴리가 없는데... ㄱㅗ$ㅈ3ㅑㅇ! ㅠㅡㅠ");
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
        //    Debug.LogError("스탭 오버플로");
   }

    //// 노드 만들어서 매개변수로 Aistatetype 지정해서 비교... 아래는 임시
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
                // 현재 레시피 순서에 맞는 재료 선택
                //Debug.Log("재료");
                CheckStep();
                break;
            }
            case AIStateType.Processing:
            {
                // 1. 고장 또는 사용중인지 / 2. 거리순
                destination = manager.objects[1].obj[0].item; // 임시
                break;
            }
            case AIStateType.Merge:
            {
                // 1. 고장 또는 사용중인지 / 2. 거리순
                destination = manager.objects[2].obj[0].item; // 임시
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
            Debug.Log("들어왔다```");
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
