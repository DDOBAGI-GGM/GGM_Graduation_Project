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
            Debug.Log("키 누름");
            GameManager.Instance.nowStageData = platform.thisStageData;     // 현재 게임매니져의 것을 내것으로 바꿔줌.       SO 는 단 한개의 것만 가지고 있음.
            platform.Interact();
        }
    }
}