using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public Transform targetPoint; // Ÿ�� ����Ʈ

    public float launchAngle = 45f; // �߻� ����
    public float gravity = 9.8f; // �߷� ���ӵ�

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
        float distance = projectileXZ.magnitude / 2; // Ÿ�ٰ� ���� ��ġ ������ �Ÿ�

        float projectileY = targetPoint.position.y - transform.position.y; // Ÿ�ٰ� ���� ��ġ�� ���� ����

        // �߻� ������ �������� ��ȯ
        float radianAngle = launchAngle * Mathf.Deg2Rad;

        // ������ � ������ ����Ͽ� �ʱ� �ӵ� ���
        float initialVelocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * radianAngle));

        // XZ ��鿡���� �ʱ� �ӵ� ���
        Vector3 velocityXZ = projectileXZ.normalized * initialVelocity;

        // Y ������ �ʱ� �ӵ� ���
        Vector3 velocityY = Vector3.up * initialVelocity * Mathf.Sin(radianAngle);

        // ���� �ʱ� �ӵ� ����
        rb.velocity = velocityXZ + velocityY;
    }
}
    