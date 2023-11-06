using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerFOV _playerFOV;

    [SerializeField] private Transform _handPos; // 손 위치

    private GameObject currentObjectInHand; // 현재 있는 오브젝트
    public GameObject CurrentObjectInHand { get { return currentObjectInHand; } set { currentObjectInHand = value; } }

    private bool is_GetIngredient = false;
    public bool Is_GetIngredient { get { return is_GetIngredient; } set { is_GetIngredient = value; } }
    private bool is_Object = false;
    public bool Is_Object { get { return is_Object; } set { is_Object = value; } }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        _playerInput.OnInteraction += PerformInteraction;           // 상호작용 이벤트 연결
    }

    private void PerformInteraction()
    {
        //Debug.Log("상호작용 키를 눌렀습니다.");
        if (!is_GetIngredient && !is_Object)
        {
            Debug.Log("테이블 오브젝트 줍기");
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
        }
    }

    private void ItemPickUpAndPuttingDown() // 오브젝트 들고 내려놓기
    {
        // if(gameObject.CompareTag("Object"))
        // {
        if (currentObjectInHand == null) // 손에 아무것도 들고 있지 않을 때
        {
            // 오브젝트를 들어주는 로직을 작성
            GameObject objectToPickup = _playerFOV.CheckForObjectsInView();        // 이거 추후 코드 수정하기
            Debug.Log(objectToPickup);
            if (objectToPickup != null && objectToPickup.gameObject.CompareTag("Item"))     // 아이템일 경우에만
            {
                objectToPickup.transform.position = _handPos.position; // 오브젝트를 손 위치로 이동
                objectToPickup.transform.parent = _handPos; // 오브젝트를 손 위치의 자식으로 설정
                currentObjectInHand = objectToPickup; // 손에 오브젝트를 들었다고 표시
            }
        }
     /*   else // 손에 이미 오브젝트를 들고 있을 때
        {
            // 오브젝트를 놓는 로직을 작성
            if (currentObjectInHand != null)
            {
                currentObjectInHand.transform.parent = null; // 오브젝트의 부모 설정을 해제
                currentObjectInHand = null; // 손에 들고 있는 오브젝트를 해제
            }
        }*/
        // }
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // 손에 아무것도 들고 있지 않을 때
        {
            // 오브젝트를 들어주는 로직을 작성
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // 오브젝트 가져오기
            if (objectToPickup != null)
            {
                GameObject item = objectToPickup.Interaction();
                //Debug.Log(item);
                item.transform.position = _handPos.position;        // 오브젝트 손 위치로 이동
                item.transform.parent = _handPos;       // 손의 자식으로 설정
                currentObjectInHand = item;     // 손에 들고 있음!
            }
        }
    }

    private void ItemObjectInteraction()
    {
        if (currentObjectInHand != null)        // 손에 뭘 들고 있으면
        {
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // 오브젝트 가져오기     여기서 문제가 생김 ㅇㅇ
            Debug.Log(objectToPickup);
            if (objectToPickup != null)
            {
                Debug.Log("손에 뭘 들고 있어서 오브젝트 상호작용 시작");
                objectToPickup.Interaction(currentObjectInHand);
                //Debug.Log(item);
            }
        }
    }
}
