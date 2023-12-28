using UnityEngine;

public class StagePlatform : Interactable
{
    [Header("이동할 씬")][SerializeField] private Object scene;
    protected override void Interact()
    {
        Debug.Log("이동된다");
        //LoadingSceneManager.Instance.StartLoading(scene.name);
        CloudManager.Instance?.Move(true);
        LoadingSceneManager.Instance?.ChangeLoadScene(scene.name);
    }
}
