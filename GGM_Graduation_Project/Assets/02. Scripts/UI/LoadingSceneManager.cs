using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    [Header("�ε� �����̴�")][SerializeField] private Slider _slider;

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
        //���� �� �̺�Ʈ�� ����� �ݴϴ�.

        //Debug.Log("LoadingScene�� Awake ��.");
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
        //Debug.Log("�� �̵� �̺�Ʈ ���");
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    private void OnDisable()
    {
        //Debug.Log("�� �̵� �̺�Ʈ ����");
        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + "���� ����Ǿ����ϴ�.");
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

            if (operation.progress < 0.9f)      // �ε�Ǵ� ���� Ŭ ��
            {
                Debug.Log("�ε�Ǵ� ���� �� Ŀ��");
                _slider.value = Mathf.Lerp(_slider.value, operation.progress, _time);
                if (_slider.value >= operation.progress)
                {
                    _time = 0f;
                }
            }
            else
            {
                _slider.value = Mathf.Lerp(_slider.value, 1f, _time);
                Debug.Log("�� ���⼭ �׷��Ǵ�");
                if (_slider.value == 1.0f)      // 1�ʴ� �� �ε� ������ �־�� ��.
                {
                    operation.allowSceneActivation = true;
                    CloudManager.Instance?.gameObject.SetActive(true);
                    CloudManager.Instance?.Move(false);
                    Debug.Log("�� �̵��ǰ� ���� ��������!");
                    yield break;
                }
            }
        }
    }
}
