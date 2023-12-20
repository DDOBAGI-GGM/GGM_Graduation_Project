using UnityEngine;

public class StagePlatform : Interactable
{
    [Header("�̵��� ��")][SerializeField] private Object scene;
    protected override void Interact()
    {
        LoadingSceneManager.Instance.StartLoading(scene.name);
    }
}
