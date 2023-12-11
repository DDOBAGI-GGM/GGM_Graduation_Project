using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : Interactable
{
    [Header("�̵��� ��")][SerializeField] private Object scene;
    protected override void Interact()
    {
        SceneManager.LoadScene(scene.name);
    }
}
