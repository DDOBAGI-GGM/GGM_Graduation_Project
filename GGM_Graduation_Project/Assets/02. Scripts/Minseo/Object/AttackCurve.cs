using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net;

public class AttackCurve : MonoBehaviour
{
    public List<Transform> visitedPoints = new List<Transform>(); // �� �� ������ ��ġ �־�α� 

    Rigidbody _rigidbody;

    public void MakeCurve(GameObject weapon, Transform[] transforms)
    {
        List<Vector3> pointList = new List<Vector3>();

        List<Transform> remainingPoints = new List<Transform>(transforms);
        
        int randomIndex = Random.Range(0, remainingPoints.Count); // ����
        Transform selectedPoint = remainingPoints[randomIndex]; // ������ ��ġ
        pointList.Add(selectedPoint.position);
        visitedPoints.Add(selectedPoint);
        remainingPoints.RemoveAt(randomIndex);

        Move(weapon, selectedPoint, 1f);
    }

    private void Move(GameObject weapon, Transform pointList, float time)
    {
        _rigidbody = weapon.GetComponent<Rigidbody>();

        Vector3 projectileXZ = new Vector3(pointList.position.x - transform.position.x, 0f, pointList.position.z - transform.position.z);
        float distance = projectileXZ.magnitude / 2; // Ÿ�ٰ� ���� ��ġ ������ �Ÿ�

        float radianAngle = 60 * Mathf.Deg2Rad;
        float initialVelocity = Mathf.Sqrt((distance * 9.8f) / Mathf.Sin(2 * radianAngle));

        Vector3 velocityXZ = projectileXZ.normalized * initialVelocity;
        Vector3 velocityY = Vector3.up * initialVelocity * Mathf.Sin(radianAngle);

        _rigidbody.velocity = velocityXZ + velocityY;


        weapon.AddComponent<BoxCollider>(); 
    }
}