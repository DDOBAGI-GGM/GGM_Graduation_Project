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

    [SerializeField] private Transform _handPos; // �� ��ġ

    public GameObject currentObjectInHand; // ���� �ִ� ������Ʈ

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
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
            ItemGetInteraction();
        _player.HandUp(currentObjectInHand != null ? true : false);
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // �տ� �ƹ��͵� ��� ���� ���� ����
        {
            //Debug.Log("������Ʈ ����ֱ�");
            // ������Ʈ�� ����ִ� ������ �ۼ�
            GameObject item = _playerFOV.CheckForObjectsInView();
            Debug.Log(item);
            if (item != null)
            {
                IObject objectToPickup = item.GetComponent<IObject>();           // ������Ʈ ��������
                if (objectToPickup != null)
                {
                    GameObject pickUpItem = objectToPickup.Interaction();
                    if (pickUpItem != null)
                    {
                        pickUpItem.transform.position = _handPos.position;        // ������Ʈ �� ��ġ�� �̵�
                        pickUpItem.transform.parent = _handPos;       // ���� �ڽ����� ����
                        currentObjectInHand = pickUpItem;     // �տ� ��� ����!
                    }
                }
            }
        }
        else
        {
            GameObject item = _playerFOV.CheckForObjectsInView();       // ������Ʈ ��������
            if (item != null)
            {
                if (item.gameObject.name == "MergingTable")
                {
                    MergeIngredient merge = item.GetComponent<MergeIngredient>();       // �̰͵� �� �ٲ��ֱ�
                    if (merge.Result == true)
                    {
                        return;     // �������� �����ϱ� �����ֱ�
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
                    else if (item.gameObject.name == "TrashCan" || item.gameObject.name == "MergingTable" || item.gameObject.name == "Table" || item.gameObject.name == "Cannon")      // - �̰� ã�ƿ����� ������. �� ��� ���ڰ� �ƴϸ� ���������� ��
                    {
                        objectToPickup.Interaction(currentObjectInHand);
                        currentObjectInHand = null;
                    }
                }
            }
        }
    }
}
