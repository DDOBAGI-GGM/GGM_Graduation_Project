using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;          // �÷��̾��� �̵� �ӵ�
    [SerializeField] private float gravity = -9.8f;         // �÷��̾ �ۿ��ϴ� �߷�
    [SerializeField] private float fieldOfViewAngle = 120f; // �þ߰� 
    [SerializeField] private float viewDistance = 1.5f;     // �þ� ���� ���� 

    private CharacterController _characterController;
    public bool IsGround
    {
        get => _characterController.isGrounded; // �÷��̾ ���� �ִ��� Ȯ��
    }

    private Vector2 _inputDirection;          // �÷��̾��� �Է� ����
    private Vector3 _movementVelocity;       // �÷��̾��� ������ �ӵ�
    public Vector3 MovementVelocity => _movementVelocity;

    [SerializeField] private Transform _handPos; // �� ��ġ

    private GameObject currentObjectInHand; // ���� �ִ� ������Ʈ

    private float verticalVelocity;         // ���� �ӵ� - �������� �ſ� ���
    private string closestObject = "";      // ���� ����� ������Ʈ
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
        _playerInput.OnInteraction += PerformInteraction;           // ��ȣ�ۿ� �̺�Ʈ ����
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

    private void Update()
    {
        CheckForObjectsInView();
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

    private void PerformInteraction()
    {
        Debug.Log("��ȣ�ۿ� Ű�� �������ϴ�.");
        ItemPickUpAndPuttingDown();
    }

    private void CheckForObjectsInView() // �þ߰� üũ
    {
        Vector3 playerPosition = transform.position; // �÷��̾� ��ġ
        Vector3 forward = transform.forward;

        float halfFOV = fieldOfViewAngle / 2f; // �þ߰��� �� ����

        Vector3 direction = forward; // ���� �þ�

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, viewDistance);

        // �þ� ������ �ð������� ǥ��
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
                        Debug.Log("������Ʈ �̸� : " + hit.transform.name);

                        // �Ÿ��� �� ����� ������Ʈ 
                        float distanceToCollider = Vector3.Distance(playerPosition, hit.transform.position);
                        if (distanceToCollider < closestDistance)
                        {
                            closestDistance = distanceToCollider;
                            closestObject = hit.transform.name;
                        }

                        // �׽�Ʈ�� ���� �׸�
                        Debug.DrawRay(playerPosition, (hit.transform.position - playerPosition).normalized * viewDistance, Color.red);
                    }
                }
            }
        }

        // ���� ����� ������Ʈ�� �̸��� ���
        if (!string.IsNullOrEmpty(closestObject)) // �����̰ų� NULL�� �ƴ϶��
        {
            Debug.Log("���� ����� ������Ʈ: " + closestObject);
        }
    }

    private void ItemPickUpAndPuttingDown() // ������Ʈ ��� ��������
    {
        if(gameObject.CompareTag("Object"))
        {
            if (currentObjectInHand == null) // �տ� �ƹ��͵� ��� ���� ���� ��
            {
                // ������Ʈ�� ����ִ� ������ �ۼ�
                if (!string.IsNullOrEmpty(closestObject))
                {
                    GameObject objectToPickup = GameObject.Find(closestObject);
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
        }
    }
}