using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    [Header("씬전환 효과")][SerializeField] private CloudManager cloud;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            cloud.Move();
        }
    }
}
