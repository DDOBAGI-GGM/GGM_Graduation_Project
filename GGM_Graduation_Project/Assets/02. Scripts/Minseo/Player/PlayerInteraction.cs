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

    private GameObject currentObjectInHand; // ���� �ִ� ������Ʈ
    public GameObject CurrentObjectInHand { get { return currentObjectInHand; } set { currentObjectInHand = value; } }        // �̰ſ� ���ؼ� �����غ���

    //private bool is_GetIngredient = false;      // �������� �������°�
    //private bool is_Object = false;     // ���� ������Ʈ�� �� �տ� ����ִ°�
    //public bool Is_Object { get { return is_Object; } set { is_Object = value; } }          // �̰� ������Ʈ���� ��ȣ�ۿ��� �� �� ���� �� true �� �Ǵ� ����!

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        //_playerInput.OnInteraction += PerformInteraction;           // ��ȣ�ۿ� �̺�Ʈ ����
        //_otherPlayerInput.OnInteraction += PerformInteraction;

        if (_playerInput == null)
            _otherPlayerInput.OnInteraction += PerformInteraction;

        if (_otherPlayerInput == null)
            _playerInput.OnInteraction += PerformInteraction;
    }

    private void PerformInteraction()
    {
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
        //if (currentObjectInHand == null)
        //{
            ItemGetInteraction();
        _player.HandUp(currentObjectInHand != null ? true : false);
        //}
        /* if (!is_GetIngredient && !is_Object)
         {
             //Debug.Log("���̺� ������Ʈ�� �ݰų� �����뿡 ��Ḧ �ΰų� �������� �ϼ�ǰ�� �������ų�");
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
         }*/
    }

    private void ItemPickUpAndPuttingDown() // ������Ʈ ��� ��������     ���̺��� �������� ���
    {
        // if(gameObject.CompareTag("Object"))
        // {
            GameObject objectToPickup = _playerFOV.CheckForObjectsInView();
        if (currentObjectInHand == null && objectToPickup != null) // �տ� �ƹ��͵� ��� ���� ���� ��
        {
            // ������Ʈ�� ����ִ� ������ �ۼ�
            //Debug.Log(objectToPickup);
            if (objectToPickup != null && objectToPickup.gameObject.CompareTag("Item"))     // �������� ��쿡��
            {
                objectToPickup.transform.position = _handPos.position; // ������Ʈ�� �� ��ġ�� �̵�
                objectToPickup.transform.parent = _handPos; // ������Ʈ�� �� ��ġ�� �ڽ����� ����
                currentObjectInHand = objectToPickup; // �տ� ������Ʈ�� ����ٰ� ǥ��
            }
        }
        /*else if (objectToPickup != null)       // �տ� �� ��� �ְ� ���ݲ��� �۾���(����) �̸�
        {
            if (objectToPickup.gameObject.name == "MergingTable")
            {
                //Debug.Log("�տ� �� ��� �ְ� ���ݲ��� �۾����� ����");
                IObject merge = objectToPickup.GetComponent<IObject>();
                if (merge != null)      // ���� �ƴѰ�쿡��
                {
                    merge.Interaction(currentObjectInHand);
                }
            }
        }*/
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
                    //Debug.Log(objectToPickup);
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
                    //Debug.Log("�������̺� ���� ���ݴ�");
                    MergeIngredient merge = item.GetComponent<MergeIngredient>();       // �̰͵� �� �ٲ��ֱ�
                    if (merge.Result == true)
                    {
                        return;     // �������� �����ϱ� �����ֱ�
                    }
                }
                if (item.gameObject.name == "Table")
                {
                    Debug.Log("���̺��� �� ���̺� ���� ���� �� �ְ� ���� �� �ְ�");
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
                        Debug.Log("������ ���⼭");
                        objectToPickup.Interaction(currentObjectInHand);
                    }
                    else if (item.gameObject.name == "TrashCan")      // - �̰� ã�ƿ����� ������. �� ��� ���ڰ� �ƴϸ� ���������� ��
                    {
                        Debug.Log("��������");
                        objectToPickup.Interaction(currentObjectInHand);
                        currentObjectInHand = null;
                    }
                }
            }
        }
    }

    private void ItemObjectInteraction()        // ���̺� �δ� ���� ����, ��������� ���մ뿡��
    {
        if (currentObjectInHand != null)        // �տ� �� ��� ������
        {
            IObject objectToPickup = _playerFOV.CheckForObjectsInView().GetComponent<IObject>();           // ������Ʈ ��������     ���⼭ ������ ���� ����
            //Debug.Log(objectToPickup);
            if (objectToPickup != null)
            {
                //Debug.Log("�տ� �� ��� �־ ������Ʈ ��ȣ�ۿ� ����");
                objectToPickup.Interaction(currentObjectInHand);
                //Debug.Log(item);
            }
        }
    }
}
