using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("로딩 패널 캔버스")][SerializeField] private CanvasGroup _loadingPanel;
    [Header("로딩 슬라이더")][SerializeField] private Slider _slider;
    private float _time;

    public static LoadingSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void StartLoading(string sceneName)
    {
        _loadingPanel.DOFade(1, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            StartCoroutine(LoadAsyncSceneCoroutine(sceneName));
        });
    }

    public IEnumerator LoadAsyncSceneCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            _time += Time.time / 100f;

            _slider.value = _time / 10f;

            if (_time > 10)
            {
                operation.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
