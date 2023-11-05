using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;          // �÷��̾��� �̵� �ӵ�
    [SerializeField] private float gravity = -9.8f;         // �÷��̾ �ۿ��ϴ� �߷�

    private CharacterController _characterController;

    public bool IsGround
    {
        get => _characterController.isGrounded; // �÷��̾ ���� �ִ��� Ȯ��
    }

    private Vector2 _inputDirection;          // �÷��̾��� �Է� ����
    private Vector3 _movementVelocity;       // �÷��̾��� ������ �ӵ�
    public Vector3 MovementVelocity => _movementVelocity;

    private float verticalVelocity;         // ���� �ӵ� - �������� �ſ� ���

    // Ű���带 ����Ͽ� ������ �� �ִ��� ����
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
        _playerInput.OnMovement += SetPlayerMovement;               // �̵� �̺�Ʈ ����
    }

    private void FixedUpdate()
    {
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        ApplyGravity();  // �߷� 
        Move();          // �̵�
    }

    // �Է��� ������� �÷��̾��� �̵� ������ ����
    public void SetPlayerMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y) * (moveSpeed * Time.fixedDeltaTime);

        if (_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity); // ������ ������ �ٶ󺸰� ��
        }
    }

    // ��� ����
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void ApplyGravity()
    {
        if (IsGround && verticalVelocity < 0)  // ���� ������ ����
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        _movementVelocity.y = verticalVelocity;
    }

    private void Move()
    {
        if (_activeMove)
        {
            _characterController.Move(_movementVelocity);
        }
    }
}