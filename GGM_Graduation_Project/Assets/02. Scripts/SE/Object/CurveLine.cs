using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveLine : MonoBehaviour
{
    [SerializeField]
    private Transform startTrm, endTrm, startControlTrm, endControlTrm;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            List<Vector3> pointList = new List<Vector3>();

            DOCurve.CubicBezier.GetSegmentPointCloud(pointList, startTrm.position, startControlTrm.position, endTrm.position, endControlTrm.position, 20);

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());

            foreach (Vector3 p in pointList)
            {
                Debug.Log(p);
            }

            // 이 라인을 통해서 큐브가 움직이게 하잠
            StartCoroutine(Move(pointList, 2f));
        }
    }

    private IEnumerator Move(List<Vector3> pointList, float time)
    {
        float animateTime = time / pointList.Count;
        foreach (Vector3 p in pointList)
        {
            yield return new WaitForSeconds(animateTime);
            //transform.position = p;
            transform.DOMove(p, animateTime).SetEase(Ease.InOutCirc);
        }
    }
}
