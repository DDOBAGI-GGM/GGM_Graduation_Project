using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;

    private Ray ray;
    private RaycastHit hitInfo;
    private StagePlatform platform;

    private void Start()
    {
        ray = new Ray(transform.position, Vector3.down);
    }

    private void Update()
    {
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.TryGetComponent<StagePlatform>(out platform))
            {
                GameManager.Instance.nowStageData = platform.thisStage;     // ���� ���ӸŴ����� ���� �������� �ٲ���.       SO �� �� �Ѱ��� �͸� ������ ����.
                if (Input.GetKeyDown(KeyCode.F))
                {
                    platform.Interact();
                }
            }
        }
    }
}