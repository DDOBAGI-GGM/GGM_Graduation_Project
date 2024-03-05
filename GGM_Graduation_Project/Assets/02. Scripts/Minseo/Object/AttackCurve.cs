using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackCurve : MonoBehaviour
{
    public List<Transform> visitedPoints = new List<Transform>(); // 한 번 공격한 위치 넣어두기 

    public void MakeCurve(GameObject weapon, Transform[] transforms)
    {
        Debug.Log("MakeCurve");
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
        Debug.Log("Move");
        Vector3 center = (gameObject.transform.position + pointList.position) * 0.5f;

        center.y -= 3;

        Vector3 startPos = gameObject.transform.position - center;
        Vector3 endPos = pointList.position - center;

        weapon.transform.position = Vector3.Slerp(startPos, endPos, time);
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


// 배지호 곡선 마냥 사용해주면 됨.