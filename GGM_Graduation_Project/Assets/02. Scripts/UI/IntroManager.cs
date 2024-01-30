using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // �׽�Ʈ������
        if (Input.GetKeyDown(KeyCode.Return) && !Input.GetKey(KeyCode.Escape))        // �׽�Ʈ������, ����â�� ���� esc Ű�� �ȉ�!
        {
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");
        }
    }   // �� �ڵ� �Ⱦ� ��?

    public void OnStartBtn()
    {
        CloudManager.Instance.Move(true);
        LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");
    }

    public void OnSetting()
    {
        SettingManager.Instance.OnSettingShow();
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
