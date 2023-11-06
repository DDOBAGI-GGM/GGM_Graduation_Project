using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerFOV _playerFOV;

    [SerializeField] private Transform _handPos; // �� ��ġ

    private GameObject currentObjectInHand; // ���� �ִ� ������Ʈ
    public GameObject CurrentObjectInHand { get { return currentObjectInHand; } set { currentObjectInHand = value; } }

    public bool is_GetIngredient = false;
    public bool is_Object = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        _playerInput.OnInteraction += PerformInteraction;           // ��ȣ�ۿ� �̺�Ʈ ����
    }

    private void PerformInteraction()
    {
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
        if (!is_GetIngredient && !is_Object)
        {
            ItemPickUpAndPuttingDown();         // �ٴ�? ���ִ� ������Ʈ�� �ݴ� ��.
        }
        else if (is_GetIngredient)
        {
            ItemGetInteraction();            // ������Ʈ�� ��� ���ִ� ��. IngredientBox ���� ���.
        }
        else if (is_Object)
        {
            // ������Ʈ�� ��� �־ � ������Ʈ ��ȣ�ۿ��� ��
            Debug.Log("dsf");
            ItemObjectInteraction();
        }
    }

    private void ItemPickUpAndPuttingDown() // ������Ʈ ��� ��������
    {
        // if(gameObject.CompareTag("Object"))
        // {
        if (currentObjectInHand == null) // �տ� �ƹ��͵� ��� ���� ���� ��
        {
            // ������Ʈ�� ����ִ� ������ �ۼ�
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                GameObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView());        // �̰� ���� �ڵ� �����ϱ�
                if (objectToPickup != null)
                {
                    objectToPickup.transform.position = _handPos.position; // ������Ʈ�� �� ��ġ�� �̵�
                    objectToPickup.transform.parent = _handPos; // ������Ʈ�� �� ��ġ�� �ڽ����� ����
                    currentObjectInHand = objectToPickup; // �տ� ������Ʈ�� ����ٰ� ǥ��
                }
            }
        }
        else // �տ� �̹� ������Ʈ�� ��� ���� ��
        {
            // ������Ʈ�� ���� ������ �ۼ�
            if (currentObjectInHand != null)
            {
                currentObjectInHand.transform.parent = null; // ������Ʈ�� �θ� ������ ����
                currentObjectInHand = null; // �տ� ��� �ִ� ������Ʈ�� ����
            }
        }
        // }
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // �տ� �ƹ��͵� ��� ���� ���� ��
        {
            // ������Ʈ�� ����ִ� ������ �ۼ�
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                IObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView()).GetComponent<IObject>();           // ������Ʈ ��������
                Debug.Log(objectToPickup);
                if (objectToPickup != null)
                {
                    GameObject item = objectToPickup.Interaction();
                    Debug.Log(item);
                    item.transform.position = _handPos.position;        // ������Ʈ �� ��ġ�� �̵�
                    item.transform.parent = _handPos;       // ���� �ڽ����� ����
                    currentObjectInHand = item;     // �տ� ��� ����!
                }
            }
        }
    }

    private void ItemObjectInteraction()
    {
        if (currentObjectInHand != null)        // �տ� �� ��� ������
        {
            if (!string.IsNullOrEmpty(_playerFOV.CheckForObjectsInView()))
            {
                IObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView()).GetComponent<IObject>();           // ������Ʈ ��������
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
