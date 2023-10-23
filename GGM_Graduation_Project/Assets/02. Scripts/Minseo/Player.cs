using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _gravity = -9.8f;

    private float _gravityMultiplier = 1f;

    private CharacterController _characterController;
    public bool IsGround
    {
        get => _characterController.isGrounded;
    }

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;

    // Ű����� �����̴� �����ΰ�?
    private bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private PlayerInput _playerInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.OnMovement += SetPlayerMovement;
        _playerInput.OnInteraction += PerformInteraction;
    }

    // ����� PlayerInput���� ���� ó���� ����.
    public void SetPlayerMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y) * (_moveSpeed * Time.fixedDeltaTime);

        if (_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity); // ������ ������ ���� �ϰ�
        }
    }

    // ��� ����
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    // ���� �ٸ� ��ũ��Ʈ���� �̵��� �ǵ帮�� �Ѵٸ� ���
    public void SetMovement(Vector3 value)
    {
        _movementVelocity = new Vector3(value.x, 0, value.z);
        _verticalVelocity = value.y;
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)  //���� ���� ����
        {
            _verticalVelocity = -1f;
        }
        else
        {
            _verticalVelocity += _gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }

        _movementVelocity.y = _verticalVelocity;
    }

    private void Move()
    {
        if (_activeMove)
        {
            _characterController.Move(_movementVelocity);
        }
    }

    private void FixedUpdate()
    {
        //Ű����� �����϶��� �̷��� �����̰�
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        ApplyGravity(); //�߷� ����
        Move();
    }

    private void PerformInteraction()
    {
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
    }
}
