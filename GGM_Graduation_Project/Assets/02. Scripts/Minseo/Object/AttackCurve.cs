using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net;

public class AttackCurve : MonoBehaviour
{
    public List<Transform> visitedPoints = new List<Transform>(); // 한 번 공격한 위치 넣어두기 

    Rigidbody _rigidbody;

    public void MakeCurve(GameObject weapon, Transform[] transforms)
    {
        Debug.Log("MakeCurve");
        Debug.Log(weapon.name);
        List<Vector3> pointList = new List<Vector3>();

        List<Transform> remainingPoints = new List<Transform>(transforms);
        
        int randomIndex = Random.Range(0, remainingPoints.Count); // 랜덤
        Transform selectedPoint = remainingPoints[randomIndex]; // 선택한 위치
        pointList.Add(selectedPoint.position);
        visitedPoints.Add(selectedPoint);
        remainingPoints.RemoveAt(randomIndex);

        Move(weapon, selectedPoint, 1f);
        //StartCoroutine(Move(weapon, selectedPoint, 1f));

        /*
        List<Vector3> pointList = new List<Vector3>();

        DOCurve.CubicBezier.GetSegmentPointCloud(pointList, transforms[0].position, transforms[1].position, transforms[2].position, transforms[3].position, 20);

        StartCoroutine(Move(weapon, pointList, 1f));
        */
    }

    //private IEnumerator Move(GameObject weapon, List<Vector3> pointList, float time)
    private void Move(GameObject weapon, Transform pointList, float time)
    {
        _rigidbody = weapon.GetComponent<Rigidbody>();

        Vector3 projectileXZ = new Vector3(pointList.position.x - transform.position.x, 0f, pointList.position.z - transform.position.z);
        float distance = projectileXZ.magnitude / 2; // 타겟과 현재 위치 사이의 거리

        float radianAngle = 45 * Mathf.Deg2Rad;
        float initialVelocity = Mathf.Sqrt((distance * 9.8f) / Mathf.Sin(2 * radianAngle));

        Vector3 velocityXZ = projectileXZ.normalized * initialVelocity;
        Vector3 velocityY = Vector3.up * initialVelocity * Mathf.Sin(radianAngle);

        _rigidbody.velocity = velocityXZ + velocityY;


        weapon.AddComponent<BoxCollider>(); 

        /*
        float animateTime = time / pointList.Count;
        foreach (Vector3 p in pointList)
        {
            yield return new WaitForSeconds(animateTime);
            weapon.transform.DOMove(p, animateTime).SetEase(Ease.InOutElastic);
        }
        */
    }
}