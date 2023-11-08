using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    [SerializeField] private float fieldOfViewAngle = 120f; // �þ߰� 
    [SerializeField] private float viewDistance = 1.5f;     // �þ� ���� ���� 

    private GameObject closestObject;      // ���� ����� ������Ʈ

    private void Update()
    {
        //CheckForObjectsInView();
    }

    public GameObject CheckForObjectsInView() // �þ߰� üũ
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
                        //Debug.Log("������Ʈ �̸� : " + hit.transform.name);

                        // �Ÿ��� �� ����� ������Ʈ 
                        float distanceToCollider = Vector3.Distance(playerPosition, hit.transform.position);
                        if (distanceToCollider < closestDistance)
                        {
                            closestDistance = distanceToCollider;
                            closestObject = hit.collider.gameObject;
                        }

                        // �׽�Ʈ�� ���� �׸�
                        Debug.DrawRay(playerPosition, (hit.transform.position - playerPosition).normalized * viewDistance, Color.red);
                    }
                }
            }
        }

        // ���� ����� ������Ʈ�� �̸��� ���
        if (closestObject != null) // �����̰ų� NULL�� �ƴ϶��
        {
            //Debug.Log("���� ����� ������Ʈ: " + closestObject.name);
            if (closestObject.name == "Table")      // ���̺��� ���
            {
                Table table = closestObject.GetComponent<Table>();          // ��״� ����ؼ� �������� �Ŵϱ� �������ֱ� ���̺��� ���������� �����
                if (table != null)
                {
                     //Debug.Log(table.Is_existObject);
                    if (table.Is_existObject && table.Interactive)
                    {
                        closestObject = table.Interaction();
                    }
                }
            }
            else if (closestObject.name == "MergingTable")      // ���� �������̺��� ���� �ϼ� ǰ�̶��
            {
                MergeIngredient merge = closestObject.GetComponent<MergeIngredient>();
                if (merge != null)
                {
                    //Debug.Log(merge.Result);
                    if (merge.Result && merge.Interactive)       // �������� ������
                    {
                        closestObject = merge.Interaction();
                    }
                }
            }
            //Debug.Log(closestObject);
            return closestObject;
        }

        return null;        // ������ ��üũ ���ִϱ�.   
    }

/*#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Vector3 playerPosition = transform.position; // �÷��̾� ��ġ
            Vector3 forward = transform.forward;

            float halfFOV = fieldOfViewAngle / 2f; // �þ߰��� �� ����

            // �þ� ������ �ð������� ǥ��
            Vector3 leftBoundary = Quaternion.Euler(0, -halfFOV, 0) * forward;
            Vector3 rightBoundary = Quaternion.Euler(0, halfFOV, 0) * forward;
            //Debug.DrawRay(playerPosition, leftBoundary * viewDistance, Color.green);
            Gizmos.DrawLine(playerPosition, leftBoundary * viewDistance);
            //Debug.DrawRay(playerPosition, rightBoundary * viewDistance, Color.green);
            Gizmos.DrawLine(playerPosition, rightBoundary * viewDistance);
            Gizmos.color = Color.green;
        }
    }
#endif*/
}
