using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    [Header("����ȯ ȿ��")][SerializeField] private CloudManager cloud;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // �׽�Ʈ������
        if (Input.anyKey)        // �׽�Ʈ������
        {
            cloud.Move();
            LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
