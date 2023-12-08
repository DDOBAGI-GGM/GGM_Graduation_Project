using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroSystem : MonoBehaviour
{
    [Header("페이드 이미지")][SerializeField] private Image _fadePanel;
    [Header("이동할 씬")][SerializeField] private Object _nextScene;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _fadePanel.DOFade(1, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                SceneManager.LoadScene(_nextScene.name);
            });
        }
    }
}
