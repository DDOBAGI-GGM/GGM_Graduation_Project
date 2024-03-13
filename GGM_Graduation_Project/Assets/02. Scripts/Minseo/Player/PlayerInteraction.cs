using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField] 
    private OtherPlayerInput _otherPlayerInput;

    private Player _player;
    private PlayerFOV _playerFOV;

    [SerializeField] private Transform _handPos; // 손 위치

    private GameObject currentObjectInHand; // 현재 있는 오브젝트
    public GameObject CurrentObjectInHand { get { return currentObjectInHand; } set { currentObjectInHand = value; } }        // 이거에 대해서 수정해보기

    //private bool is_GetIngredient = false;      // 아이템을 가져오는것
    //private bool is_Object = false;     // 지금 오브젝트가 내 손에 들려있는가
    //public bool Is_Object { get { return is_Object; } set { is_Object = value; } }          // 이건 오브젝트여서 상호작용을 할 수 있을 때 true 가 되는 것임!

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        //_playerInput.OnInteraction += PerformInteraction;           // 상호작용 이벤트 연결
        //_otherPlayerInput.OnInteraction += PerformInteraction;

        if (_playerInput == null)
            _otherPlayerInput.OnInteraction += PerformInteraction;

        if (_otherPlayerInput == null)
            _playerInput.OnInteraction += PerformInteraction;
    }

    private void PerformInteraction()
    {
        Debug.Log("상호작용 키를 눌렀습니다.");
        //if (currentObjectInHand == null)
        //{
            ItemGetInteraction();
        _player.HandUp(currentObjectInHand != null ? true : false);
        //}
        /* if (!is_GetIngredient && !is_Object)
         {
             //Debug.Log("테이블 오브젝트를 줍거나 머지대에 재료를 두거나 머지대의 완성품을 가져오거나");
             ItemPickUpAndPuttingDown();         // 바닥? 에있는 오브젝트를 줍는 것.
         }
         else if (is_GetIngredient)
         {
             ItemGetInteraction();            // 오브젝트를 얻게 해주는 것. IngredientBox 에서 사용.
         }
         else if (is_Object)
         {
             // 오브젝트를 들고 있어서 어떤 오브젝트 상호작용할 때
             //Debug.Log("dsf");
             ItemObjectInteraction();
         }*/
    }

    private void ItemPickUpAndPuttingDown() // 오브젝트 들고 내려놓기     테이블에서 가져오는 경우
    {
        // if(gameObject.CompareTag("Object"))
        // {
            GameObject objectToPickup = _playerFOV.CheckForObjectsInView();
        if (currentObjectInHand == null && objectToPickup != null) // 손에 아무것도 들고 있지 않을 때
        {
            // 오브젝트를 들어주는 로직을 작성
            //Debug.Log(objectToPickup);
            if (objectToPickup != null && objectToPickup.gameObject.CompareTag("Item"))     // 아이템일 경우에만
            {
                objectToPickup.transform.position = _handPos.position; // 오브젝트를 손 위치로 이동
                objectToPickup.transform.parent = _handPos; // 오브젝트를 손 위치의 자식으로 설정
                currentObjectInHand = objectToPickup; // 손에 오브젝트를 들었다고 표시
            }
        }
        /*else if (objectToPickup != null)       // 손에 뭘 들고 있고 지금꺼가 작업대(머지) 이면
        {
            if (objectToPickup.gameObject.name == "MergingTable")
            {
                //Debug.Log("손에 뭘 들고 있고 지금꺼가 작업대일 때만");
                IObject merge = objectToPickup.GetComponent<IObject>();
                if (merge != null)      // 널이 아닌경우에만
                {
                    merge.Interaction(currentObjectInHand);
                }
            }
        }*/
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // 손에 아무것도 들고 있지 않을 때만
        {
            //Debug.Log("오브젝트 들어주기");
            // 오브젝트를 들어주는 로직을 작성
            GameObject item = _playerFOV.CheckForObjectsInView();
            Debug.Log(item);
            if (item != null)
            {
                IObject objectToPickup = item.GetComponent<IObject>();           // 오브젝트 가져오기
                if (objectToPickup != null)
                {
                    //Debug.Log(objectToPickup);
                    GameObject pickUpItem = objectToPickup.Interaction();
                    if (pickUpItem != null)
                    {
                        pickUpItem.transform.position = _handPos.position;        // 오브젝트 손 위치로 이동
                        pickUpItem.transform.parent = _handPos;       // 손의 자식으로 설정
                        currentObjectInHand = pickUpItem;     // 손에 들고 있음!
                    }
                }
            }
        }
        else
        {
            GameObject item = _playerFOV.CheckForObjectsInView();       // 오브젝트 가져오기
            if (item != null)
            {
                if (item.gameObject.name == "MergingTable")
                {
                    //Debug.Log("머지테이블에 뭐가 있잖니");
                    MergeIngredient merge = item.GetComponent<MergeIngredient>();       // 이것들 다 바꿔주기
                    if (merge.Result == true)
                    {
                        return;     // 리솔츠가 있으니까 멈춰주기
                    }
                }
                if (item.gameObject.name == "Table")
                {
                    Debug.Log("테이블일 때 테이블에 뭐가 있을 수 있고 없을 수 있고");
                    Table table = item.GetComponent<Table>();
                    if (table.Is_existObject == true)
                    {
                        return;
                    }
                }

                IObject objectToPickup = item.GetComponent<IObject>();
                if (objectToPickup != null)      
                {
                    if (item.gameObject.name == "ProcessingIngredient")
                    {
                        Debug.Log("가공은 여기서");
                        objectToPickup.Interaction(currentObjectInHand);
                    }
                    else if (item.gameObject.name == "TrashCan")      // - 이게 찾아와지지 않으면. 즉 재료 상자가 아니면 쓰레기통일 때
                    {
                        Debug.Log("쓰레기통");
                        objectToPickup.Interaction(currentObjectInHand);
                        currentObjectInHand = null;
                    }
                }
            }
        }
    }

    private void ItemObjectInteraction()        // 테이블에 두는 경우와 무기, 쓰레기통과 조합대에서
    {
        if (currentObjectInHand != null)        // 손에 뭘 들고 있으면
        {
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // 오브젝트 가져오기     여기서 문제가 생김 ㅇㅇ
            //Debug.Log(objectToPickup);
            if (objectToPickup != null)
            {
                //Debug.Log("손에 뭘 들고 있어서 오브젝트 상호작용 시작");
                objectToPickup.Interaction(currentObjectInHand);
                //Debug.Log(item);
            }
        }
    }
}
