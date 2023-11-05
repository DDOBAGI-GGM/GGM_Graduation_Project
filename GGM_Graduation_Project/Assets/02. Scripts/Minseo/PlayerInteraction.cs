using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerFOV _playerFOV;

    [SerializeField] private Transform _handPos; // �� ��ġ

    private GameObject currentObjectInHand; // ���� �ִ� ������Ʈ

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerFOV = GetComponent<PlayerFOV>(); 
        _playerInput.OnInteraction += PerformInteraction;           // ��ȣ�ۿ� �̺�Ʈ ����
    }

    private void PerformInteraction()
    {
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
        ItemPickUpAndPuttingDown();
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
                GameObject objectToPickup = GameObject.Find(_playerFOV.CheckForObjectsInView());
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
}
