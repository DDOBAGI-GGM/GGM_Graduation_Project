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
                GameManager.Instance.nowStageData = platform.thisStage;     // 현재 게임매니져의 것을 내것으로 바꿔줌.       SO 는 단 한개의 것만 가지고 있음.
                if (Input.GetKeyDown(KeyCode.F))
                {
                    platform.Interact();
                }
            }
        }
    }
}