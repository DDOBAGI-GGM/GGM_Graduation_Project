using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;          // 플레이어의 이동 속도
    [SerializeField] private float gravity = -9.8f;         // 플레이어에 작용하는 중력

    private CharacterController _characterController;

    public bool IsGround
    {
        get => _characterController.isGrounded; // 플레이어가 땅에 있는지 확인
    }

    private Vector2 _inputDirection;          // 플레이어의 입력 방향
    private Vector3 _movementVelocity;       // 플레이어의 움직임 속도
    public Vector3 MovementVelocity => _movementVelocity;

    private float verticalVelocity;         // 수직 속도 - 떨어지는 거에 사용

    // 키보드를 사용하여 움직일 수 있는지 여부
    private bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private OtherPlayerInput _otherPlayerInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();            
        //_playerInput.OnMovement += SetPlayerMovement;               // 이동 이벤트 연결
        //_otherPlayerInput.OnMovement += SetPlayerMovement;

        if (_playerInput == null)
            _otherPlayerInput.OnMovement += SetPlayerMovement;               // 이동 이벤트 연결

        if (_otherPlayerInput == null)
            _playerInput.OnMovement += SetPlayerMovement;

    }

    private void FixedUpdate()
    {
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        ApplyGravity();  // 중력 
        Move();          // 이동
    }

    // 입력을 기반으로 플레이어의 이동 방향을 설정
    public void SetPlayerMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y) * (moveSpeed * Time.fixedDeltaTime);

        if (_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity); // 가야할 방향을 바라보게 함
        }
    }

    // 즉시 멈춤
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    private void ApplyGravity()
    {
        if (IsGround && verticalVelocity < 0)  // 땅에 착지한 상태
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