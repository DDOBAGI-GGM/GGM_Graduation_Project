using UnityEngine;

public class StagePlatform : Interactable
{
    [Header("�̵��� ��")][SerializeField] private Object scene;
    protected override void Interact()
    {
        Debug.Log("�̵��ȴ�");
        //LoadingSceneManager.Instance.StartLoading(scene.name);
        CloudManager.Instance?.Move(true);
        LoadingSceneManager.Instance?.ChangeLoadScene(scene.name);
    }
}
