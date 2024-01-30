using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject gamePausePanel, settingPanel;
    [SerializeField] private Animator fadeAnim;

    [SerializeField] private bool is_NowShow = false;       // 확인용
    [SerializeField] private bool is_GamePause = false;         // 게임펄스 창이 켜져 있냐 판단하는 불값.
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

    public void OnSettingBackBtn()      // 셋팅화면만 나갈 때
    {
        ShowPanel(!is_NowShow, settingPanel);
    }

    public void OnGamePauseBackBtn()        // 게임 화면만 나갈 때
    {
        ShowPanel(!is_NowShow, gamePausePanel);
    }

    public void OnSettingShow()     // 게임 멈춤에서 사용하는 것.  인트로에서도 찾아와서 사용해줌.
    {
        ShowPanel(true, settingPanel);
    }

    private IEnumerator AddFadeAnim(bool is_PanelShow, GameObject panel)
    {
        if (panel == gamePausePanel)
        {
            is_GamePause = is_PanelShow;
        }

        //Debug.Log("아");
        if (is_PanelShow)
        {
            is_NowShow = true;
        }

        Time.timeScale = 1;     // 애니가 보여야 하니까. 

        if (is_PanelShow == true && is_GamePause)
        {
            Debug.Log("플레이어 멈추고 타이머 멈추고 해주십숑");
        }

        is_Fading = true;
        FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // 애니메이션 재생 시간

        panel.SetActive(is_PanelShow);

        FadeAnim(false);
        yield return new WaitForSeconds(0.5f);

        is_Fading = false;

        if (is_PanelShow == false)
        {
            if (is_GamePause == false)
            {
                is_NowShow = false; 
                Debug.Log("플레이어 멈추고 타이머 멈춘거 풀어주십숑");
            }
            else
            {
                Time.timeScale = 0;     // 게임이 멈춘 상태니까.
            }
        }
        else
        {
            Time.timeScale = 0;     // 설정창이든 뭐든 창이 켜진 것이면
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
