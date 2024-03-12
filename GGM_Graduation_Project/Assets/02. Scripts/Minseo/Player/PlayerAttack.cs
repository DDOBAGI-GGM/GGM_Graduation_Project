using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private OtherPlayerInput _otherPlayerInput;

    // �����ϴ� ��ũ��Ʈ

    private void Awake()
    {
        if (_playerInput == null)
            _otherPlayerInput.OnAttack += PerformAttack;

        if (_otherPlayerInput == null)
            _playerInput.OnInteraction += PerformAttack;
    }

    private void PerformAttack()
    {
        Debug.Log("����������");
    }
}
