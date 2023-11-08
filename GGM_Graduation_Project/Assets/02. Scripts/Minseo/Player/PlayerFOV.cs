using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    [SerializeField] private float fieldOfViewAngle = 120f; // 시야각 
    [SerializeField] private float viewDistance = 1.5f;     // 시야 범위 길이 

    private GameObject closestObject;      // 가장 가까운 오브젝트

    private void Update()
    {
        //CheckForObjectsInView();
    }

    public GameObject CheckForObjectsInView() // 시야각 체크
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
                        //Debug.Log("오브젝트 이름 : " + hit.transform.name);

                        // 거리가 더 가까운 오브젝트 
                        float distanceToCollider = Vector3.Distance(playerPosition, hit.transform.position);
                        if (distanceToCollider < closestDistance)
                        {
                            closestDistance = distanceToCollider;
                            closestObject = hit.collider.gameObject;
                        }

                        // 테스트를 위한 그림
                        Debug.DrawRay(playerPosition, (hit.transform.position - playerPosition).normalized * viewDistance, Color.red);
                    }
                }
            }
        }

        // 가장 가까운 오브젝트의 이름을 출력
        if (closestObject != null) // 공백이거나 NULL이 아니라면
        {
            //Debug.Log("가장 가까운 오브젝트: " + closestObject.name);
            if (closestObject.name == "Table")      // 테이블일 경우
            {
                Table table = closestObject.GetComponent<Table>();          // 얘네는 계속해서 가져오는 거니까 변경해주기 테이블이 여러개인지 물어보고
                if (table != null)
                {
                     //Debug.Log(table.Is_existObject);
                    if (table.Is_existObject && table.Interactive)
                    {
                        closestObject = table.Interaction();
                    }
                }
            }
            else if (closestObject.name == "MergingTable")      // 만약 머지테이블에서 나온 완성 품이라면
            {
                MergeIngredient merge = closestObject.GetComponent<MergeIngredient>();
                if (merge != null)
                {
                    //Debug.Log(merge.Result);
                    if (merge.Result && merge.Interactive)       // 리솔츠가 있으면
                    {
                        closestObject = merge.Interaction();
                    }
                }
            }
            //Debug.Log(closestObject);
            return closestObject;
        }

        return null;        // 위에서 널체크 해주니까.   
    }

/*#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Vector3 playerPosition = transform.position; // 플레이어 위치
            Vector3 forward = transform.forward;

            float halfFOV = fieldOfViewAngle / 2f; // 시야각의 반 각도

            // 시야 범위를 시각적으로 표시
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
