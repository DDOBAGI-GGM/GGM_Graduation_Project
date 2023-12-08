using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public AI brain;

    protected StateMachine<AIController> fsmManager;
    protected BehaviourTreeManager<INode> btManager;

    public NavMeshAgent navAgent;

    //public LayerMask targetLayerMask;
    public Transform target;
    //public float eyeSight;

    public bool tArride = false;

    private void Awake()
    {
        brain = GetComponent<AI>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //상태관리자 생성 및 상태 추가
        fsmManager = new StateMachine<AIController>(this, new StopState());
        fsmManager.AddStateList(new MoveState());

        btManager = new BehaviourTreeManager<INode>();
        btManager.SetRoot(new SelectorNode
            (new SequenceNode(/*조리법 선택*/
                new ConditionNode(haveRecipe),
                new ActionNode(ChoiceRecipe)),
            new SequenceNode(/*재료 선택 준비*/
                new ConditionNode(haveHand),
                new MoveNode(move),
                new ActionNode(useTable)),
            /*재료 선택*/
            new MoveNode(move),
            new ActionNode(ChoiceIngredent),
            new SequenceNode(/*가공 하기*/
                new ConditionNode(needMake),
                new ActionNode(make)),
            /*조리대 넣기*/
            new ActionNode(putItem)));
    }

    private void Update()
    {
        fsmManager.Update(Time.deltaTime);
        btManager.Update();
    }

    bool move()
    {
        tArride = false;
        target = brain.objects[0].transform;
        fsmManager.ChangeState<MoveState>();

        if (tArride == true)
            return true;
        return false;
    }

    // 레시피 가지고 있는지
    bool haveRecipe()
    {
        Debug.Log("조합법 검사");
        if (brain.recipe != null)
            return false;
        return true;
    }

    // 조리법 선택
    void ChoiceRecipe()
    {
        Debug.Log("조합법 선택");
        brain.recipe = brain.recipes[0];
    }

    // 빈 손인지
    bool haveHand()
    {
        Debug.Log("빈 손 검사");
        if (brain.hand != null)
            return true;
        return false;   
    }

    // 선반 사용
    void useTable()
    {
        Debug.LogError("선반 사용");
        //tArride = false;
        //// 선반 찾고
        //target = brain.objects[0].transform;
        //// 움직여라 얍
        //fsmManager.ChangeState<MoveState>();

        //if (tArride)
        //{
        //    // 멈춰!
        //    fsmManager.ChangeState<StopState>();
        //    if (brain.hand == null)
        //        Debug.Log("손이 비었는데 선반 사용이 들어옴");
        //    else
        //    {
        //        //선반사용! -> 선반 없으니까 일단 삭제 갈겨버려
        //        Destroy(brain.hand.GetComponentInChildren<GameObject>());
        //    }
        //}
    }

    // 재료 선택
    void ChoiceIngredent()
    {
        if (tArride)
        {
            Debug.Log("재료 선택");
            IObject objectToPickup = target.GetComponent<IObject>();           // 오브젝트 가져오기
            if (objectToPickup != null)
            {
                //Debug.Log(objectToPickup);
                GameObject pickUpItem = objectToPickup.Interaction();
                if (pickUpItem != null)
                {
                    pickUpItem.transform.position = brain.handPos.position;        // 오브젝트 손 위치로 이동
                    pickUpItem.transform.parent = brain.handPos;       // 손의 자식으로 설정
                    brain.hand = pickUpItem;     // 손에 들고 있음!

                    return;
                }
            }
        }
    }

    // 가공 필요한가
    bool needMake()
    {
        Debug.Log("가공 절차 검사");
        return true;
    }

    void make()
    {
        Debug.Log("가공 하기");

    }

    void putItem()
    {
        Debug.Log("조리대 넣기");

    }
}