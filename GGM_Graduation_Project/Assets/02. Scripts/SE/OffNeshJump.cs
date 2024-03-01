using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OffNeshJump : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed = 10.0f;
    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private int offMexhAreaNumber = 2;      // Jump ���̾�

    [SerializeField] private GameObject airplane;

    private NavMeshAgent navAgent;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => IsOnJump());
            yield return StartCoroutine(JumpTo());
        }
    }

    IEnumerator JumpTo()
    {
        navAgent.isStopped = true;
        OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;
        Vector3 start = transform.position;
        Vector3 end = linkData.endPos;

        float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);
        float currenTime = 0;
        float percent = 0;

        float v0 = (end - start).y - gravity;   // y ���� �ʱ� �ӵ�

        while (percent < 1)
        {
            // ������ � : ������ġ + �ʱ�ӵ� * �ð� + �߷� * �ð�������
            currenTime += Time.deltaTime;
            percent = currenTime / jumpTime;

            Vector3 pos = Vector3.Lerp(start, end, percent);
            pos.y = start.y + (v0 * percent) + (gravity * percent * percent);
            pos.y = Mathf.Clamp(pos.y, 1, 5);

            transform.position = pos;
            yield return null;
        }

        navAgent.CompleteOffMeshLink();
        navAgent.isStopped = false;

        airplane.transform.SetParent(null);
        airplane.transform.position = new Vector3(airplane.transform.position.x, 0.35f, airplane.transform.position.z);
    }

    private bool IsOnJump()
    {
        if (navAgent.isOnOffMeshLink)       // ��ũ Ÿ�� (���� �ϸ�)
        {
            OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;

            if (linkData.offMeshLink != null && linkData.offMeshLink.area == offMexhAreaNumber)
            {
                airplane.transform.SetParent(gameObject.transform);
                airplane.transform.localPosition = Vector3.zero;
                return true;
            }
        }
        return false;
    }
}
