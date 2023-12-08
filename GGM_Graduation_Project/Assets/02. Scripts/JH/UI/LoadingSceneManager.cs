using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("로딩 슬라이더")][SerializeField] private Slider _slider;
    [Header("로딩할 씬")][SerializeField] private Object _nextScene;

    private float _time;

    private void Start()
    {
        StartCoroutine(LoadAsyncSceneCoroutine());
    }

    private IEnumerator LoadAsyncSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_nextScene.name);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            _time += Time.time;

            _slider.value = _time / 10f;

            if (_time > 10)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
