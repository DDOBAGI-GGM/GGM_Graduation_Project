using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : Interactable
{
    [Header("¿Ãµø«“ æ¿")][SerializeField] private Object scene;
    protected override void Interact()
    {
        SceneManager.LoadScene(scene.name);
    }
}
