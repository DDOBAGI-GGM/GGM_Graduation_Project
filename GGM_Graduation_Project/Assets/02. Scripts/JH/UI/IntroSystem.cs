using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // 테스트용으로
        if (Input.GetKeyDown(KeyCode.Return) && !Input.GetKey(KeyCode.Escape))        // 테스트용으로, 설정창을 여는 esc 키는 안됭!
        {
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
