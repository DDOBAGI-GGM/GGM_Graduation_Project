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

    public GameObject currentObjectInHand; // 현재 있는 오브젝트

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerFOV = GetComponent<PlayerFOV>(); 

        if (_playerInput == null)
            _otherPlayerInput.OnInteraction += PerformInteraction;

        if (_otherPlayerInput == null)
            _playerInput.OnInteraction += PerformInteraction;
    }

    private void OnDisable()
    {
        if (_playerInput == null)
            _otherPlayerInput.OnInteraction -= PerformInteraction;

        if (_otherPlayerInput == null)
            _playerInput.OnInteraction -= PerformInteraction;
    }

    private void PerformInteraction()
    {
        Debug.Log("상호작용 키를 눌렀습니다.");
            ItemGetInteraction();
        _player.HandUp(currentObjectInHand != null ? true : false);
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
                    MergeIngredient merge = item.GetComponent<MergeIngredient>();       // 이것들 다 바꿔주기
                    if (merge.Result == true)
                    {
                        return;     // 리솔츠가 있으니까 멈춰주기
                    }
                }
                if (item.gameObject.name == "Table")
                {
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
                        objectToPickup.Interaction(currentObjectInHand);
                    }
                    else if (item.gameObject.name == "TrashCan" || item.gameObject.name == "MergingTable" || item.gameObject.name == "Table" || item.gameObject.name == "Cannon")      // - 이게 찾아와지지 않으면. 즉 재료 상자가 아니면 쓰레기통일 때
                    {
                        objectToPickup.Interaction(currentObjectInHand);
                        currentObjectInHand = null;
                    }
                }
            }
        }
    }
}
