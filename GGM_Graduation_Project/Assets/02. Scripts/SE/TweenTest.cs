using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    [SerializeField]
    private Transform startTrm, endTrm, startControlTrm, endControlTrm;

    private LineRenderer lineRenderer;

    private Transform coinTrm;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        coinTrm = transform.Find("CoinTemplate");
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

            // �� ������ ���ؼ� ť�갡 �����̰� ����
            StartCoroutine(Move(pointList, 2f));
        }

        // ���� �����ϴ� ��
        if (Input.GetKeyDown(KeyCode.O))        // ���λ�����
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos = Random.insideUnitCircle * 2f;
                //pos.y = 0;
                Vector3 targetPos = transform.position + new Vector3(pos.x, 0, pos.y);

                Transform trm = Instantiate(coinTrm, transform);
                trm.gameObject.SetActive(true);
                //trm.DOJump(transform.position + (Vector3)pos, 4f, 1, 1.2f).SetEase(Ease.InQuad);
                trm.DOJump(targetPos, 4f, 1, 1.2f).SetEase(Ease.InQuad);
            }
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
