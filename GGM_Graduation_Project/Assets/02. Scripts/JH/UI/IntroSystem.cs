using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    [Header("����ȯ ȿ��")][SerializeField] private CloudManager cloud;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            cloud.Move();
        }
    }
}
