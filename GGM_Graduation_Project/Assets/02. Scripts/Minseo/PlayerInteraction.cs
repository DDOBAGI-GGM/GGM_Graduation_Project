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

    private bool is_GetIngredient = false;
    public bool Is_GetIngredient { get { return is_GetIngredient; } set { is_GetIngredient = value; } }
    private bool is_Object = false;
    public bool Is_Object { get { return is_Object; } set { is_Object = value; } }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        _playerInput.OnInteraction += PerformInteraction;           // ��ȣ�ۿ� �̺�Ʈ ����
    }

    private void PerformInteraction()
    {
        //Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
        if (!is_GetIngredient && !is_Object)
        {
            Debug.Log("���̺� ������Ʈ �ݱ�");
            ItemPickUpAndPuttingDown();         // �ٴ�? ���ִ� ������Ʈ�� �ݴ� ��.
        }
        else if (is_GetIngredient)
        {
            ItemGetInteraction();            // ������Ʈ�� ��� ���ִ� ��. IngredientBox ���� ���.
        }
        else if (is_Object)
        {
            // ������Ʈ�� ��� �־ � ������Ʈ ��ȣ�ۿ��� ��
            //Debug.Log("dsf");
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
            GameObject objectToPickup = _playerFOV.CheckForObjectsInView();        // �̰� ���� �ڵ� �����ϱ�
            Debug.Log(objectToPickup);
            if (objectToPickup != null && objectToPickup.gameObject.CompareTag("Item"))     // �������� ��쿡��
            {
                objectToPickup.transform.position = _handPos.position; // ������Ʈ�� �� ��ġ�� �̵�
                objectToPickup.transform.parent = _handPos; // ������Ʈ�� �� ��ġ�� �ڽ����� ����
                currentObjectInHand = objectToPickup; // �տ� ������Ʈ�� ����ٰ� ǥ��
            }
        }
     /*   else // �տ� �̹� ������Ʈ�� ��� ���� ��
        {
            // ������Ʈ�� ���� ������ �ۼ�
            if (currentObjectInHand != null)
            {
                currentObjectInHand.transform.parent = null; // ������Ʈ�� �θ� ������ ����
                currentObjectInHand = null; // �տ� ��� �ִ� ������Ʈ�� ����
            }
        }*/
        // }
    }

    private void ItemGetInteraction()
    {
        if (currentObjectInHand == null) // �տ� �ƹ��͵� ��� ���� ���� ��
        {
            // ������Ʈ�� ����ִ� ������ �ۼ�
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // ������Ʈ ��������
            if (objectToPickup != null)
            {
                GameObject item = objectToPickup.Interaction();
                //Debug.Log(item);
                item.transform.position = _handPos.position;        // ������Ʈ �� ��ġ�� �̵�
                item.transform.parent = _handPos;       // ���� �ڽ����� ����
                currentObjectInHand = item;     // �տ� ��� ����!
            }
        }
    }

    private void ItemObjectInteraction()
    {
        if (currentObjectInHand != null)        // �տ� �� ��� ������
        {
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // ������Ʈ ��������     ���⼭ ������ ���� ����
            Debug.Log(objectToPickup);
            if (objectToPickup != null)
            {
                Debug.Log("�տ� �� ��� �־ ������Ʈ ��ȣ�ۿ� ����");
                objectToPickup.Interaction(currentObjectInHand);
                //Debug.Log(item);
            }
        }
    }
}
