using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(LineRenderer))]
public class AttackCurve : MonoBehaviour
{
    //private LineRenderer lineRenderer;

    //private void Start()
    //{
    //    lineRenderer = GetComponent<LineRenderer>();
    //}

    public void MakeCurve(GameObject weapon, Transform[] transforms)
    {
        List<Vector3> pointList = new List<Vector3>();

        DOCurve.CubicBezier.GetSegmentPointCloud(pointList, transforms[0].position, transforms[1].position, transforms[2].position, transforms[3].position, 20);

        //lineRenderer.positionCount = pointList.Count;
        //lineRenderer.SetPositions(pointList.ToArray());

        // �� ������ ���ؼ� ť�갡 �����̰� ����
        StartCoroutine(Move(weapon, pointList, 2f));
    }

    private IEnumerator Move(GameObject weapon, List<Vector3> pointList, float time)
    {
        float animateTime = time / pointList.Count;
        foreach (Vector3 p in pointList)
        {
            yield return new WaitForSeconds(animateTime);
            weapon.transform.DOMove(p, animateTime).SetEase(Ease.InOutCirc);
        }
        Debug.Log("���� �� ��ƼŬ ������ֱ�!");
    }
}


// ����ȣ � ���� ������ָ� ��.