using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // 테스트용으로
        if (Input.anyKey)        // 테스트용으로
        {
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
