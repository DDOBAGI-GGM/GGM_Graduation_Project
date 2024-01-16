using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    [Header("로딩 슬라이더")][SerializeField] private Slider _slider;

    private bool is_Loading = false;
    public static string changeScene = "";
    private float _time;

/*    public static LoadingSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
        //시작 시 이벤트를 등록해 줍니다.

        //Debug.Log("LoadingScene의 Awake 야.");
    }*/

    private void Update()
    {
        if (is_Loading)
        {
            _slider = FindObjectOfType<Slider>();
            CloudManager.Instance?.gameObject.SetActive(false);
            StartLoading();
            is_Loading = false;
        }
    }

    private void OnEnable()
    {
        //Debug.Log("씬 이동 이벤트 등록");
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    private void OnDisable()
    {
        //Debug.Log("씬 이동 이벤트 삭제");
        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + "으로 변경되었습니다.");
        if (SceneManager.GetActiveScene().name == "Loading_Scene")
        {
            is_Loading = true;
        }
    }

    public void ChangeLoadScene(string change)
    {
        changeScene = change;
    }

    public void StartLoading()
    {
        StartCoroutine(LoadAsyncSceneCoroutine());
    }

    public IEnumerator LoadAsyncSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(changeScene);
        operation.allowSceneActivation = false;

        //while (!operation.isDone)
        //{
        //    _time += Time.deltaTime;
        //    _time += 0.1f;

        //    _slider.value = _time / 3f;

        //    if (_time > 3)
        //    {
        //        CloudManager.Instance?.gameObject.SetActive(true);
        //        CloudManager.Instance?.Move(false);
        //        operation.allowSceneActivation = true;
        //    }

        //    yield return new WaitForSeconds(0.1f);
        //}

        while (!operation.isDone)
        {
            yield return null;

            _time += Time.deltaTime;

            if (operation.progress < 0.9f)      // 로드되는 맵이 클 때
            {
                Debug.Log("로드되는 맵이 좀 커서");
                _slider.value = Mathf.Lerp(_slider.value, operation.progress, _time);
                if (_slider.value >= operation.progress)
                {
                    _time = 0f;
                }
            }
            else
            {
                _slider.value = Mathf.Lerp(_slider.value, 1f, _time);
                Debug.Log("아 여기서 그래되니");
                if (_slider.value == 1.0f)      // 1초는 이 로딩 씬에서 있어야 해.
                {
                    operation.allowSceneActivation = true;
                    CloudManager.Instance?.gameObject.SetActive(true);
                    CloudManager.Instance?.Move(false);
                    Debug.Log("씬 이동되고 구름 보여지기!");
                    yield break;
                }
            }
        }
    }
}
