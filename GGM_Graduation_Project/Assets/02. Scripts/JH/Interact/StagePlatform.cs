using UnityEngine;

public class StagePlatform : Interactable
{
    [Header("¿Ãµø«“ æ¿")][SerializeField] private Object scene;
    protected override void Interact()
    {
        LoadingSceneManager.Instance.StartLoading(scene.name);
    }
}
