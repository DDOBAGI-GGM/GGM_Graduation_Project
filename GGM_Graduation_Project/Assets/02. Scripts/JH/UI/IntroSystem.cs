using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    [Header("씬전환 효과")][SerializeField] private CloudManager cloud;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))        // 테스트용으로
        if (Input.anyKey)        // 테스트용으로
        {
            cloud.Move();
            LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");
        }
    }
}
