using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public StagePlatform platform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && platform != null)
        {
            Debug.Log("Ű ����");
            GameManager.Instance.nowStageData = platform.thisStageData;     // ���� ���ӸŴ����� ���� �������� �ٲ���.       SO �� �� �Ѱ��� �͸� ������ ����.
            platform.Interact();
        }
    }
}