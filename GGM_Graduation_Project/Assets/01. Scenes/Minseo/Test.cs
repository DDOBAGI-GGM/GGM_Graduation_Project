using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public Transform targetPoint; // 타겟 포인트

    public float launchAngle = 45f; // 발사 각도
    public float gravity = 9.8f; // 중력 가속도

    private Rigidbody rb;

    private void Start()
    {
        
        Launch();
    }

    private void Launch()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();

        Vector3 projectileXZ = new Vector3(targetPoint.position.x - transform.position.x, 0f, targetPoint.position.z - transform.position.z);
        float distance = projectileXZ.magnitude / 2; // 타겟과 현재 위치 사이의 거리

        float projectileY = targetPoint.position.y - transform.position.y; // 타겟과 현재 위치의 높이 차이

        // 발사 각도를 라디안으로 변환
        float radianAngle = launchAngle * Mathf.Deg2Rad;

        // 포물선 운동 공식을 사용하여 초기 속도 계산
        float initialVelocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * radianAngle));

        // XZ 평면에서의 초기 속도 계산
        Vector3 velocityXZ = projectileXZ.normalized * initialVelocity;

        // Y 방향의 초기 속도 계산
        Vector3 velocityY = Vector3.up * initialVelocity * Mathf.Sin(radianAngle);

        // 최종 초기 속도 설정
        rb.velocity = velocityXZ + velocityY;
    }
}
    