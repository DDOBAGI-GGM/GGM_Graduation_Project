using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // �׽�Ʈ������
        if (Input.GetKeyDown(KeyCode.Return) && !Input.GetKey(KeyCode.Escape))        // �׽�Ʈ������, ����â�� ���� esc Ű�� �ȉ�!
        {
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
