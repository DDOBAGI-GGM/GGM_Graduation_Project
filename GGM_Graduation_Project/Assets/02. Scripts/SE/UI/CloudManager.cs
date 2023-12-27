using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloudManager : Singleton<CloudManager>
{
    [SerializeField] Cloud[] clouds = new Cloud[1];

    [SerializeField] private Image panel;
    [SerializeField] private float sceneChangeTime = 1.0f;
    [SerializeField] private bool is_panelNow = false;
    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private float animTime = 1;
    private float nowTime = 0.0f;
    private bool is_SceneChange = false;
    public bool Is_SceneChange { get { return is_SceneChange; } private set { } }

/*    public static CloudManager Instance;        // �̱������� ������ְ�

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }*/

    private void Start()
    {
        if (is_panelNow)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1f);       // ����� �׳� ����̸� 1, 1, 1, 1�� �ص���.
            panel.DOFade(0, fadeTime).OnComplete(() =>
            {
                //DontShow();
                for (int i = 0; i < clouds.Length; i++)
                {
                    clouds[i].Show(false, animTime);
                }
                is_panelNow = false;
            });
        }
        else
        {
            for (int i = 0; i < clouds.Length; i++)
            {
                clouds[i].Init();
            }
        }
    }

    public void Move(bool is_goOrigin)      // Ʈ��� �������� ��ġ�� ���ƿ���, �޽��� �ٱ����� �з�������
    {
        if (is_goOrigin)
        {
            for (int i = 0; i < clouds.Length; i++)
            {
                clouds[i].Show(true, animTime);
            }
            is_SceneChange = true;
        }
        else
        {
            panel.DOFade(0, fadeTime).OnComplete(() =>
            {
                //DontShow();
                for (int i = 0; i < clouds.Length; i++)
                {
                    clouds[i].Show(false, animTime);
                }
                is_panelNow = false;
            });
        }
    }

    private void Update()
    {
  /*      if (Input.GetKeyDown(KeyCode.N))
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1f);       // ����� �׳� ����̸� 1, 1, 1, 1�� �ص���.
            panel.DOFade(0, fadeTime).OnComplete(() =>
            {
                //DontShow();
                for (int i = 0; i < clouds.Length; i++)
                {
                    clouds[i].Show(false, animTime);
                }
                is_panelNow = false;
            });
        }       // �� ģ���� ��¥ ����׿� �ڵ���.

        if (Input.GetKeyDown(KeyCode.Y))
        {
            for (int i = 0; i < clouds.Length; i++)
            {
                clouds[i].Show(true, animTime);
            }
            is_SceneChange = true;
        }*/

        if (is_SceneChange)
        {
            nowTime += Time.deltaTime;
            if (nowTime > sceneChangeTime)
            {
                is_SceneChange=false;
                nowTime = 0;
                panel.DOFade(1, fadeTime).OnComplete(() => SceneManager.LoadScene("Loading_Scene"));
            }
        }
    }
}
