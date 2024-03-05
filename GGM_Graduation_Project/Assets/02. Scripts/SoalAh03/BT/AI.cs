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
            // 3차 - Dev

            // 레시피 선택 + 수리
            new SequenceNode
            (
                // 레시피가 없다면
                new ConditionNode(NullRecipe),
                // 레시피를 지정한다
                new RecipeNode(this),
                // 레시피가 있다면 (레시피가 지정됐다면)
                new InverterNode(new ConditionNode(NullRecipe)),
                // 레시피 로그 출력
                new LogNode("레시피"),
                // 재료 상태로 변경
                new ChangeStateNode(this, AIStateType.Ingredient)
            ),

                new AnimationNode(this),
            // 목적지 설정 및 이동
            new SequenceNode
            (
                // 목적지가 없다면 (상태 변경이 일어났다면)
                new ConditionNode(NullDestination),
                // 목적지를 설정
                new DestinationNode(this),
                // 이동
                new InverterNode(new RangeNode(this)),
                new LogNode("이동"), 
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
                // 상호작용
                    new WaitNode(1.5f),
                new InteractionNode(this),
                // 빈 손이 아니라면 (아이템 획득)
                new InverterNode(new ConditionNode(HandNull)),
                //new WaitNode(1.5f),
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
                    //new WaitNode(1.5f),
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
                    //new WaitNode(1.5f),
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
                    // 상호작용
                        new WaitNode(1.5f),
                    new InteractionNode(this),
                    //new WaitNode(1.5f),
                    // 상태 로그 출력
                    new LogNode("병합"),
                    // 빈 손이라면 (아이템 부착)
                    new ConditionNode(HandNull),
                    // 수리
                    new SelectorNode
                    (
                        // 하나 넣었을 때
                        new SequenceNode
                        (
                            new InverterNode(new ConditionNode(pick)),
                            new ActionNode(NextStep)
                            //new WaitNode(1f)
                        ),
                        // 두 개 넣었을 때
                        new SequenceNode
                        (
                            new ConditionNode(pick),
                            new ActionNode(NextStep),
                            new ActionNode(EndRecovery)
                        )
                    )
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
                    //new WaitNode(1f),
                    // 상호작용
                        new WaitNode(1.5f),
                    new InteractionNode(this),
                    //new WaitNode(1.5f),
                    // 빈 손이 아니라면 (아이템 획득)
                    new InverterNode(new ConditionNode(HandNull)),
                    // 상태 로그 출력
                    new LogNode("획득"),
                    // 선반 or (공격 or 폐기)
                    new SelectorNode
                    (
                        // 선반
                        new SequenceNode
                        (
                            // 선반 사용이라면 (현재 수리으로 검사)
                            new ConditionNode(Recovery),
                            // 근데 건진 게 쓰레기면 안 됨;;
                            new InverterNode(new ConditionNode(CheckTrash)),
                            new SelectorNode
                            (
                                // 선반 보관
                                new SequenceNode
                                (
                                    new ConditionNode(canShelf),
                                    new LogNode("일단 선반에 쟁여둬"),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                ),
                                // 병합 넣기
                                new SequenceNode
                                (
                                    new ConditionNode(canMerge),
                                    new InverterNode(new ConditionNode(HandNull)),
                                        new WaitNode(1.5f),         
                                    new InteractionNode(this),
                                    //new WaitNode(1.5f), 
                                    new LogNode("일단 다시 넣어둬"),
                                    new ActionNode(NextRecipe),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                )
                            )
                        ),
                        // 공격-폐기 상태 검사
                        new SelectorNode
                        (
                            new InverterNode(new ConditionNode(Recovery)),
                                new LogNode("111111111111111111111"),
                            // 공격
                            new SequenceNode
                            (
                                // 쓰레기가 아니라면
                                new InverterNode(new ConditionNode(CheckTrash)),
                                // 정보 로그 출력
                                new LogNode("아이템"),
                                new ActionNode(ClearState),
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
                                new ActionNode(ClearState),
                                // 폐기 상태로 변경
                                new ChangeStateNode(this, AIStateType.Trash)
                            )
                        )
                    )
                )
            ),

            // 선반
            new SequenceNode
            (
                // 공격 스탭이 맞다면
                new CheckStateNode(this, AIStateType.Shelf),
                // 거리가 된다면,
                new RangeNode(this),
                new SelectorNode
                (
                    // 회수
                    new SequenceNode
                    (
                        new ConditionNode(canMerge),
                        new ConditionNode(HandNull),
                            new WaitNode(1.5f),         
                        new InteractionNode(this),
                        new LogNode(" 선반 - 회수"),
                        new InverterNode(new ConditionNode(HandNull)),
                        new ChangeStateNode(this, AIStateType.Merge),
                        new ActionNode(ClearState),
                        new LogNode("회수 성공")
                    ),
                    // 보관
                    new SequenceNode
                    (
                        new ConditionNode(canShelf),
                        new InverterNode(new ConditionNode(HandNull)),
                            new WaitNode(1.5f),         
                        new InteractionNode(this),
                        new LogNode(" 선반 - 보관"),
                        new ConditionNode(HandNull),
                        new ActionNode(NextStep),
                        new ActionNode(NextRecipe)
                    )
                )
            ),

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
                    new WaitNode(1.5f),           
                new InteractionNode(this),
                //new WaitNode(1.5f),
                // 상호작용 성공
                new ConditionNode(HandNull),
                // 상태 로그 출력
                new LogNode(" 공격"),
                new ActionNode(test),
                // 레시피 초기화
                new ActionNode(ClearRecipe)
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
                    new WaitNode(1.5f),           
                new InteractionNode(this),
                //new WaitNode(1.5f),
                // 상호작용 성공
                new ConditionNode(HandNull),
                // 상태 로그 출력
                new LogNode(" 폐기"),
                // 레시피 초기화
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
