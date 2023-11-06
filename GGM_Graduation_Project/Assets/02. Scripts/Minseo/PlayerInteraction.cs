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

    public bool is_GetIngredient = false;
    public bool is_Object = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        _playerInput.OnInteraction += PerformInteraction;           // 상호작용 이벤트 연결
    }

    private void PerformInteraction()
    {
        Debug.Log("상호작용 키를 눌렀습니다.");
        if (!is_GetIngredient && !is_Object)
        {
            ItemPickUpAndPuttingDown();         // 바닥? 에있는 오브젝트를 줍는 것.
        }
        else if (is_GetIngredient)
        {
            ItemGetInteraction();            // 오브젝트를 얻게 해주는 것. IngredientBox 에서 사용.
        }
        else if (is_Object)
        {
            // 오브젝트를 들고 있어서 어떤 오브젝트 상호작용할 때
            Debug.Log("dsf");
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
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                GameObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView());        // 이거 추후 코드 수정하기
                if (objectToPickup != null)
                {
                    objectToPickup.transform.position = _handPos.position; // 오브젝트를 손 위치로 이동
                    objectToPickup.transform.parent = _handPos; // 오브젝트를 손 위치의 자식으로 설정
                    currentObjectInHand = objectToPickup; // 손에 오브젝트를 들었다고 표시
                }
            }
        }
        else // 손에 이미 오브젝트를 들고 있을 때
        {
            // 오브젝트를 놓는 로직을 작성
            if (currentObjectInHand != null)
            {
                currentObjectInHand.transform.parent = null; // 오브젝트의 부모 설정을 해제
                currentObjectInHand = null; // 손에 들고 있는 오브젝트를 해제
            }
        }
        // }
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // 손에 아무것도 들고 있지 않을 때
        {
            // 오브젝트를 들어주는 로직을 작성
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                IObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView()).GetComponent<IObject>();           // 오브젝트 가져오기
                Debug.Log(objectToPickup);
                if (objectToPickup != null)
                {
                    GameObject item = objectToPickup.Interaction();
                    Debug.Log(item);
                    item.transform.position = _handPos.position;        // 오브젝트 손 위치로 이동
                    item.transform.parent = _handPos;       // 손의 자식으로 설정
                    currentObjectInHand = item;     // 손에 들고 있음!
                }
            }
        }
    }

    private void ItemObjectInteraction()
    {
        if (currentObjectInHand != null)        // 손에 뭘 들고 있으면
        {
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                IObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView()).GetComponent<IObject>();           // 오브젝트 가져오기
                Debug.Log(objectToPickup);
                if (objectToPickup != null)
                {
                    objectToPickup.Interaction(currentObjectInHand);
                    //Debug.Log(item);
                }
            }
        }
    }
}
