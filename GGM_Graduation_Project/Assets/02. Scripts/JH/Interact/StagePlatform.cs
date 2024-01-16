using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : Interactable
{
    [Header("이동할 씬")][SerializeField] private Object scene;
    protected override void Interact()
    {
        Debug.Log("이동된다");
        //LoadingSceneManager.Instance.StartLoading(scene.name);
        CloudManager.Instance?.Move(true);
        LoadingSceneManager.Instance?.ChangeLoadScene(scene.name);
        if (CloudManager.Instance == null && LoadingSceneManager.Instance == null)      // 디버그용 코드
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
