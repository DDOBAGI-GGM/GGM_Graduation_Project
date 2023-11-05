using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    [SerializeField] private float fieldOfViewAngle = 120f; // �þ߰� 
    [SerializeField] private float viewDistance = 1.5f;     // �þ� ���� ���� 

    private string closestObject = "";      // ���� ����� ������Ʈ

    private void Update()
    {
        CheckForObjectsInView();
    }

    public string CheckForObjectsInView() // �þ߰� üũ
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
            return closestObject;
        }

        return closestObject;   
    }

}
