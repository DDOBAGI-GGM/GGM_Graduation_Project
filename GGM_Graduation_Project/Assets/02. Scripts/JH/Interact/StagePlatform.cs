using UnityEngine;

public class StagePlatform : Interactable
{
    [Header("¿Ãµø«“ æ¿")][SerializeField] private Object scene;
    public CloudManager CloudManager;
    protected override void Interact()
    {
        //LoadingSceneManager.Instance.StartLoading(scene.name);
        CloudManager.Move();
        LoadingSceneManager.Instance.ChangeLoadScene(scene.name);
    }
}
