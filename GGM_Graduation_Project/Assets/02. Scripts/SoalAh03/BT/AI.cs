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
    // 가능하면 manager recipe 저장하는 곳에 bool 만들어서 레시피 각각에서 관리하도록 바꾸기!
    // -> 보류. 선반 사용하려나?..


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
            // 3차 - Dev
            // 레시피 선택
            new SequenceNode
            (
                // 레시피가 없다면
                //new ConditionNode(NullRecipe),
                // 레시피를 지정한다
                //new ActionNode(GiveRecipe),
                new RecipeNode(this),
                // 레시피가 있다면 (레시피가 지정됐다면)
                //new InverterNode(new ConditionNode(NullRecipe)),
                // 레시피 로그 출력
                new LogNode("레시피"),
                // 레시피 초기화
                new ActionNode(ResetRecipe),
                // 재료 상태로 변경
                new ChangeStateNode(this, AIStateType.Ingredient)
            ),

            // 목적지 설정 및 이동
            new SequenceNode
            (
                // 목적지가 없다면 (상태 변경이 일어났다면)
                new ConditionNode(NullDestination),
                // 목적지를 설정
                new DestinationNode(this),
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new DestinationNode(this)
                //),
                // 이동
                new MoveNode(this, 3f)
            ),

            // 재료 선택
            new SequenceNode
            (
                // 재료 스탭이 맞다면
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
                new ChangeStateNode(this, AIStateType.Processing)
            ),

            // 가공 (가공 검사, 가공)
            new SelectorNode
            (
                // 가공 검사
                new SequenceNode
                (
                    // 가공 스탭이 맞다면
                    new CheckStateNode(this, AIStateType.Processing),
                    // 아이템을 들고 있다면
                    new InverterNode(new ConditionNode(HandNull)),
                    // 가공이 필요하지 않다면
                    new InverterNode(new ConditionNode(NeedProcessing)),
                    // 상태 초기화
                    new ActionNode(ClearState),
                    // 병합 상태로 변경
                    new ChangeStateNode(this, AIStateType.Merge)
                ),

                // 가공
                new SequenceNode
                (
                    // 가공 스탭이 맞다면
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
                    new ActionNode(NextStep)
                ),

                // 회수
                new SequenceNode
                (
                    // 병합 스탭이 맞다면
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
                        //// 회복 - test .ver (recovery인 경우...)
                        //new SequenceNode
                        //(
                        //    // 일단 회복 그걸 만드는 중 일 때만...
                        //    new ConditionNode(Recovery),
                        //    // 근데 건진 게 쓰레기면 안 됨;;
                        //    new InverterNode(new ConditionNode(CheckTrash)),
                        //    new SelectorNode
                        //    (
                        //        // 선반
                        //        new SequenceNode
                        //        (
                        //            new ConditionNode(Recovery1),
                        //            new LogNode("일단 선반에 쟁여둬"),
                        //            new ChangeStateNode(this, AIStateType.Shelf)
                        //        ),
                        //        // 선반은 아니고... 회복이라서 ... .ㅎ.ㅎ.ㅎ
                        //        new SequenceNode
                        //        (
                        //            new ConditionNode(Recovery2),
                        //            new InteractionNode(this)
                        //        )
                        //    )
                        //),
                        new SequenceNode(
                            new InverterNode(new ConditionNode(Recovery)),
                        // 공격
                        new SequenceNode
                        (
                            // 쓰레기가 아니라면
                            new InverterNode(new ConditionNode(CheckTrash)),
                            // 정보 로그 출력
                            new LogNode("아이템"),
                            // 공격 상태로 변경
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
                            new ChangeStateNode(this, AIStateType.Trash)
                        )
                        )
                    )
                )
            ),

            //// 선반
            //new SequenceNode
            //(
            //    // 선반에 두러 오는 것과 회수 하러 오는 것을 구분해야함.)
            //    // 공격 스탭이 맞다면
            //    new CheckStateNode(this, AIStateType.Shelf),
            //    // 거리가 된다면,
            //    new RangeNode(this),
            //    new SelectorNode
            //    (
            //        // 회수
            //        new SequenceNode
            //        (
            //            new ConditionNode(Recovery2),
            //            new ConditionNode(HandNull),
            //            new InteractionNode(this),
            //            new LogNode(" 선반 - 회수"),
            //            new InverterNode(new ConditionNode(HandNull)),
            //            new ChangeStateNode(this, AIStateType.Merge)
            //        ),
            //        // 보관
            //        new SequenceNode
            //        (   
            //            new ConditionNode(Recovery1),
            //            new InverterNode(new ConditionNode(HandNull)),
            //            new InteractionNode(this),
            //            new LogNode(" 선반 - 보관"),
            //            new ConditionNode(HandNull),
            //            new ActionNode(NextRecipe)
            //        )
            //    )
            //    // 아이템을 들고 있다면
            //    // 상호작용
            //    // 상호작용 성공
            //    // 상태 로그 출력
            //    // 레시피 초기화
            //),

            // 공격
            new SequenceNode
            (
                // 공격 스탭이 맞다면
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
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new ActionNode(NextRecipe)
                //)
            ),

            // 폐기
            new SequenceNode
            (
                // 폐기 스탭이 맞다면
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
