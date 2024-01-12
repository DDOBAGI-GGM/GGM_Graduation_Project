using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject gamePausePanel, settingPanel;
    [SerializeField] private Animator fadeAnim;

    private bool is_NowShow = false;
    private bool is_Fading = false;     // 페이드 애니가 재생되고 있는 거 판단ㄴ

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading_Scene" /*&& CloudManager.Instance?.Is_SceneChange == false*/ && is_Fading == false)      // 로딩중이 아닐때, 애니메이션도 안돌아갈 때, 주석은 테스트 용으로 구냥
        {
            if (SceneManager.GetActiveScene().buildIndex >= 4)       // 대충 4 이상부터 스테이지씬을 넣어놨다고 가정했을 때
            {
                // 인게임설정창 1? 나오게 하기
                ShowPanel(!is_NowShow, gamePausePanel);
            }
            else
            {
                Debug.Log("셋팅!");
                ShowPanel(!is_NowShow, settingPanel);
                // 바로 설정창 나오게!
            }
        }
    }

    public void ShowPanel(bool is_PanelShow, GameObject panel)
    {
        if (is_Fading) return;
        else StartCoroutine(AddFadeAnim(is_PanelShow, panel));
    }

    private IEnumerator AddFadeAnim(bool is_PanelShow, GameObject panel)
    {
        //Debug.Log("아");
        if (is_PanelShow)
        {
            is_NowShow = true;
        }

        Time.timeScale = 1;     // 애니가 보여야 하니까.

        is_Fading = true;
        FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // 애니메이션 재생 시간

        panel.SetActive(is_PanelShow);

        FadeAnim(false);
        yield return new WaitForSeconds(0.5f);

        is_Fading = false;

        if (is_PanelShow == false)
        {
            is_NowShow = false; 
        }
        else
        {
            Time.timeScale = 0;     // 설정창이 켜진거면
        }
    }

    public void FadeAnim(bool is_in)
    {
        if (is_in)
        {
            fadeAnim.SetTrigger("In");
        }
        else
        {
            fadeAnim.SetTrigger("Out");
        }
    }
}
