using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("�ε� �����̴�")][SerializeField] private Slider _slider;
    public static string changeScene = "";
    private float _time;

    public static LoadingSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
        //���� �� �̺�Ʈ�� ����� �ݴϴ�.
        SceneManager.sceneLoaded += LoadedsceneEvent;
}

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + "���� ����Ǿ����ϴ�.");
        if (SceneManager.GetActiveScene().name == "Loading_Scene")
        {
            _slider = FindObjectOfType<Slider>();
            StartLoading(changeScene);
        }
    }

    public void ChangeLoadScene(string change)
    {
        changeScene = change;
    }

    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadAsyncSceneCoroutine(sceneName));
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
