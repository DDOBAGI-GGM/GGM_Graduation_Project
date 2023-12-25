using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class testcode : MonoBehaviour
{
    float _time = 0;
    public Slider _slider;

    private void Start()
    {
      //  _slider = FindObjectOfType<Slider>();
        //StartLoading("StageSelect_Scene");
    }

    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadAsyncSceneCoroutine(sceneName));
    }

    public IEnumerator LoadAsyncSceneCoroutine(string sceneName)
    {
        Debug.Log("df");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //_time += Time.deltaTime;
            _time += 0.1f;

            _slider.value = _time / 10f;

            if (_time > 10)
            {
                operation.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(0.1f);
        }

        //while (!operation.isDone)
        //{
        //    yield return null;

        //    _time += Time.deltaTime;

        //    if (operation.progress < 0.9f)      // 로드되는 맵이 클 때
        //    {
        //        Debug.Log("로드되는 맵이 좀 커서");
        //        _slider.value = Mathf.Lerp(_slider.value, operation.progress, _time);
        //        if (_slider.value >= operation.progress)
        //        {
        //            _time = 0f;
        //        }
        //    }
        //    else
        //    {
        //        _slider.value = Mathf.Lerp(_slider.value, 1f, _time);
        //        Debug.Log("아 여기서 그래되니");
        //        if (_slider.value == 1.0f)      // 1초는 이 로딩 씬에서 있어야 해.
        //        {
        //            operation.allowSceneActivation = true;
        //            CloudManager.Instance?.gameObject.SetActive(true);
        //            CloudManager.Instance?.Move(false);
        //            Debug.Log("씬 이동되고 구름 보여지기!");
        //            yield break;
        //        }
        //    }
        //}

        //yield return null;
    }
}