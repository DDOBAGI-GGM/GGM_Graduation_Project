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
    public GameObject target;
    public GameObject destination; // 테스트용
    public RecipeListSO recipe; // 테스트용
    public int recipeIdx;
    public RecipeListSO AArecipe; // 테스트용
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
        // 1차
        //bt.SetRoot(new SequenceNode
        //(
        //    new SelectorNode
        //    (
        //        new DestinationNode(this),  
        //        new MoveNode(this, 3f),
        //        //new WaitNode(1f),
        //        new SequenceNode
        //        (
        //            // 재료 선택
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
        //            //new RepeaterNode(new InteractionNode(this), false, true, 4), // 리피터로 interaction 3초 반복하는 부분(실패)
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
        //                // 아이템 완성이라면?
        //                //new InverterNode(new ConditionNode(HandNull)),
        //                new ConditionNode(aa),
        //                new LogNode("얏따"),
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
        //    //new LogNode("1회전 성공"),
        //    )
        //));

        bt.SetRoot(new SelectorNode
        (
            // 3차
            // 레시피 선택 (레시피가 비어있을 때)
            new SequenceNode
            (
                new ConditionNode(NullRecipe), // 레시피가 없다면
                new ActionNode(GiveRecipe), // 레시피를 지정한다
                new InverterNode(new ConditionNode(NullRecipe)), // 레시피가 있다면
                new ActionNode(ResetRecipe) // 레시피 시작 지점으로 이동한다
            ),

            // 목적지 설정 (1. 현재 스탭 확인, 2. 손 상태 확인)
            new SequenceNode
            (
                new ConditionNode(NullDestination),
                new ActionNode(Destination),
                new MoveNode(this, 3f)
            ),

            // 재료 선택
            new SequenceNode
            (
                // 재료 선택 스탭이 맞는지?
                new CheckStateNode(this, AIStateType.Ingredient),
                    //new ConditionNode(StepIngredient),
                // 손이 비었는지
                new ConditionNode(HandNull),
                // 상호작용
                new InteractionNode(this),
                // 제대로 손에 들어왔는지?
                new InverterNode(new ConditionNode(HandNull)),
                // 다음 단계 ㄹㅊㄱ NextStep
                new ActionNode(NextStep)
            ),

            // 가공
            new SequenceNode
            (
                // 가공 스탭이 맞는지?
                new CheckStateNode(this, AIStateType.Processing),
                    //new ConditionNode(StepProcessing), // 구 버전
                // 빈손은 아닌지
                new InverterNode(new ConditionNode(HandNull)),
                // 상호작용 (상호작용 성공했다는 가정하에 Success 반환)
                new InteractionNode(this),
                // 다음 단계 ㄹㅊㄱ NextStep
                new ActionNode(NextStep)
            ),

            // 병합
            new SequenceNode
            (
                // 병합 스탭이 맞는지?
                new CheckStateNode(this, AIStateType.Merge),
                    //new ConditionNode(StepProcessing), // 구 버전
                // 빈 손은 아닌지
                new InverterNode(new ConditionNode(HandNull)),
                // 상호작용 (상호작용 성공했다는 가정하에 Success 반환)
                new InteractionNode(this),
                // 아이템이 완성 됐는지 (상호작용 후 null이 아니라면 아이템을 꺼낸 것)
                new InverterNode(new ConditionNode(HandNull)),
                // 다음 레시피 스탭 or (공격 or 쓰레기통) (둘 중 하나만 실행됨)
                new SelectorNode
                (
                    // 다음 레시피 스탭
                    new SequenceNode
                    (
                        // 빈 손 이라면
                        new ConditionNode(HandNull),
                        // 다음 레시피 스탭을 진행한다 (state도 ingredient 설정)
                        new ActionNode(NextRecipe)
                    ),
                    // 공격 or 쓰레기통
                    new SequenceNode
                    (
                        // 빈 손이 아니라면 (아이템 획득)
                        new InverterNode(new ConditionNode(HandNull)),
                        new SelectorNode
                        (
                            // 공격
                            new SequenceNode
                            (
                                // 쓰레기가 아니라면
                                new InverterNode(new ConditionNode(CheckTrash)),
                                // 현재 State를 attack으로 변경해주고
                                new ChangeStateNode(this, AIStateType.Attack)
                            ),
                            // 쓰레기통
                            new SequenceNode
                            (
                                // 쓰레기라면
                                new InverterNode(new ConditionNode(CheckTrash)),
                                // 현재 State를 trash로 변경해주고
                                new ChangeStateNode(this, AIStateType.Trash)
                            )
                        ),
                        // 목적지 설정
                        new ActionNode(Destination),
                        // 이동
                        new MoveNode(this, 3f),
                        // 상호작용
                        new InteractionNode (this),
                        // 상호작용 성공
                        new ConditionNode(HandNull),
                        // 레시피 삭제
                        new ActionNode(ClearRecipe)
                    ) // - 공격 or 쓰레기통 일 때...
                ) // - 병합 완성이 아닐 때,...
            )

            // 3-1차
            //// 레시피 한 챕터가 끝났다면 (손이 비어있을 때)
            //new SequenceNode
            //(
            //    new ConditionNode(HandNull), // 손이 비어있다면
            //    new ActionNode(CheckStep), // 현재 레시피 단계에 맞는 목적지로 지정한다. (destination 대용...)
            //    new MoveNode(this, 3f) // 목적지에 도착했다면 
            //),
            //// 재료 선택
            //new SequenceNode
            //(
            //    new SelectorNode
            //    (
            //        new SequenceNode
            //        (
            //            new ConditionNode(HandNull),
            //            new ActionNode(HandClear),
            //            new LogNode("선반 사용")
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

            // 2차
            //new DestinationNode(this),
            //new SequenceNode
            //(
            //    // 재료 선택
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
            //        // 아이템 완성이라면?
            //        new ConditionNode(aa),
            //        new LogNode("얏따"),
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
    }

    void ResetRecipe()
    {
        stateType = AIStateType.Ingredient;
        recipeIdx = 0;
    }

    void NextRecipe()
    {
        stateType = AIStateType.Ingredient;
        recipeIdx++;
        if (recipeIdx > 2)
            Debug.LogError("레시피 스탭 초과");
    }

    void CheckStep() // ActionNode를 통해 부르지 않고 다른 함수에 의해 부르는 중 == 이걸 RecipeNode에 넣자!
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
                        Debug.LogError("목적지를 설정할 수 없음");
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
    }

    string ExtractName(string itemName)
    {
        string pattern = @"-_";
        Match match = Regex.Match(itemName, pattern);
        return match.Value;
    }

    //void ChangePro()
    //{
    //    destination = manager.objects[1].obj[0].item;
    //}

    bool NullDestination()
    {
        if (destination != null)
            return false;
        return true;
    }

   void NextStep()
   {
        destination = null;
        stateType++;
        if (((int)stateType) > (int)AIStateType.Shelf)
            Debug.LogError("스탭 오버플로");
   }

    //// 노드 만들어서 매개변수로 Aistatetype 지정해서 비교... 아래는 임시
    //bool StepIngredient()
    //{
    //    if (stateType == AIStateType.Ingredient)
    //        return true;
    //    return false;
    //}

    //bool StepProcessing()
    //{
    //    if (stateType == AIStateType.Processing)
    //        return true;
    //    return false;
    //}

    //bool StepMerge()
    //{
    //    if (stateType == AIStateType.Merge)
    //        return true;
    //    return false;
    //}

    void Destination()
    {
        switch (stateType)
        {
            case AIStateType.Ingredient:
            {
                // 현재 레시피 순서에 맞는 재료 선택
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
