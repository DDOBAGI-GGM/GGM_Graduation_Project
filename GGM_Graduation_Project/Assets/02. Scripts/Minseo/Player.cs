using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;          // 플레이어의 이동 속도
    [SerializeField] private float gravity = -9.8f;         // 플레이어에 작용하는 중력
    [SerializeField] private float fieldOfViewAngle = 120f; // 시야각 
    [SerializeField] private float viewDistance = 1.5f;     // 시야 범위 길이 

    private CharacterController _characterController;
    public bool IsGround
    {
        get => _characterController.isGrounded; // 플레이어가 땅에 있는지 확인
    }

    private Vector2 _inputDirection;          // 플레이어의 입력 방향
    private Vector3 _movementVelocity;       // 플레이어의 움직임 속도
    public Vector3 MovementVelocity => _movementVelocity;

    [SerializeField] private Transform _handPos; // 손 위치

    private GameObject currentObjectInHand; // 현재 있는 오브젝트

    private float verticalVelocity;         // 수직 속도 - 떨어지는 거에 사용
    private string closestObject = "";      // 가장 가까운 오브젝트
    // 키보드를 사용하여 움직일 수 있는지 여부
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
        _playerInput.OnMovement += SetPlayerMovement;               // 이동 이벤트 연결
        _playerInput.OnInteraction += PerformInteraction;           // 상호작용 이벤트 연결
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

    private void Update()
    {
        CheckForObjectsInView();
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

    private void PerformInteraction()
    {
        Debug.Log("상호작용 키를 눌렀습니다.");
        ItemPickUpAndPuttingDown();
    }

    private void CheckForObjectsInView() // 시야각 체크
    {
        Vector3 playerPosition = transform.position; // 플레이어 위치
        Vector3 forward = transform.forward;

        float halfFOV = fieldOfViewAngle / 2f; // 시야각의 반 각도

        Vector3 direction = forward; // 현재 시야

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, viewDistance);

        // 시야 범위를 시각적으로 표시
        Vector3 leftBoundary = Quaternion.Euler(0, -halfFOV, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, halfFOV, 0) * forward;
        Debug.DrawRay(playerPosition, leftBoundary * viewDistance, Color.green);
        Debug.DrawRay(playerPosition, rightBoundary * viewDistance, Color.green);

        float closestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            if (Vector3.Angle(direction, collider.transform.position - playerPosition) < halfFOV)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerPosition, (collider.transform.position - playerPosition).normalized, out hit, viewDistance))
                {
                    if (hit.transform.CompareTag("Object"))
                    {
                        Debug.Log("오브젝트 이름 : " + hit.transform.name);

                        // 거리가 더 가까운 오브젝트 
                        float distanceToCollider = Vector3.Distance(playerPosition, hit.transform.position);
                        if (distanceToCollider < closestDistance)
                        {
                            closestDistance = distanceToCollider;
                            closestObject = hit.transform.name;
                        }

                        // 테스트를 위한 그림
                        Debug.DrawRay(playerPosition, (hit.transform.position - playerPosition).normalized * viewDistance, Color.red);
                    }
                }
            }
        }

        // 가장 가까운 오브젝트의 이름을 출력
        if (!string.IsNullOrEmpty(closestObject)) // 공백이거나 NULL이 아니라면
        {
            Debug.Log("가장 가까운 오브젝트: " + closestObject);
        }
    }

    private void ItemPickUpAndPuttingDown() // 오브젝트 들고 내려놓기
    {
        if(gameObject.CompareTag("Object"))
        {
            if (currentObjectInHand == null) // 손에 아무것도 들고 있지 않을 때
            {
                // 오브젝트를 들어주는 로직을 작성
                if (!string.IsNullOrEmpty(closestObject))
                {
                    GameObject objectToPickup = GameObject.Find(closestObject);
                    if (objectToPickup != null)
                    {
                        objectToPickup.transform.position = _handPos.position; // 오브젝트를 손 위치로 이동
                        objectToPickup.transform.parent = _handPos; // 오브젝트를 손 위치의 자식으로 설정
                        currentObjectInHand = objectToPickup; // 손에 오브젝트를 들었다고 표시
                    }
                }
            }
            else // 손에 이미 오브젝트를 들고 있을 때
            {
                // 오브젝트를 놓는 로직을 작성
                if (currentObjectInHand != null)
                {
                    currentObjectInHand.transform.parent = null; // 오브젝트의 부모 설정을 해제
                    currentObjectInHand = null; // 손에 들고 있는 오브젝트를 해제
                }
            }
        }
    }
}