using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // �׽�Ʈ������
        if (Input.anyKey)        // �׽�Ʈ������
        {
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
