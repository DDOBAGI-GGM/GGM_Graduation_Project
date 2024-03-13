using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject gamePausePanel, settingPanel;       // 게임일때 표시되는 설정창, 셋팅 창 따로 존재함.
    [SerializeField] private Animator fadeAnim;     // 위아래로 움직이는 페이드

    private bool is_Setting = false;        // 셋팅창이 켜져있는가
    private bool is_GamePause = false;         // 게임중에 보이는 설정창
    private bool is_Fading = false;     // 페이드 애니가 재생되고 있는 거 판단

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading_Scene" /*&& CloudManager.Instance?.Is_SceneChange == false*/ && is_Fading == false)     
            // 로딩중이 아닐때, 애니메이션도 안돌아갈 때, 주석한 것은 구름이 보이고 있을 때가 아닐 때를 말함. 나중에 위로 빼도 좋을 듯.
        {
            if (SceneManager.GetActiveScene().buildIndex >= 4)       // 4 이상부터 스테이지씬을 넣어놨다고 가정했을 때
            {
                // 셋팅창이 켜져 있지 않을 때 게임설정창 나오게 하기
                if (is_Setting == false)
                {
                    ShowPanel(!is_GamePause, gamePausePanel);
                }
                else
                {
                    // 셋팅창 꺼주기
                    ShowPanel(!is_Setting, settingPanel);
                }
            }
            else
            {
                // 셋팅창 나오게 하기
                ShowPanel(!is_Setting, settingPanel);
            }
            // 2개 다 현재의 반대 상태로 함수 보내주기.
        }
    }

    public void ShowPanel(bool is_PanelShow, GameObject panel)
    {
        if (is_Fading) return;      // 페이드 중이면 리턴.
        else StartCoroutine(AddFadeAnim(is_PanelShow, panel));
    }

    private IEnumerator AddFadeAnim(bool is_PanelShow, GameObject panel)
    {
        if (panel == gamePausePanel)        // 게임 설정창이라면
        {
            is_GamePause = is_PanelShow;        // 게임 창의 상태를 설정해줌.
        }
        else if (panel == settingPanel)
        {
            is_Setting = is_PanelShow;
        }

        Time.timeScale = 1;     // 애니가 보여야 하니까. 

        if (is_PanelShow == true)       // 패널이 보이는 상태이면
        {
            Debug.Log("플레이어 멈추고 타이머 멈추고 해주십숑");
        }

        #region 보이고 안보이게, 페이드 애니 재생도.
        is_Fading = true;
        FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // 애니메이션 재생 시간

        panel.SetActive(is_PanelShow);

        FadeAnim(false);
        yield return new WaitForSeconds(0.5f);
        is_Fading = false;
        #endregion

        if (is_PanelShow == false)      // 패널이 꺼진 것이면 시간은 흐르게,
        {
           Debug.Log("플레이어 멈추고 타이머 멈춘거 풀어주십숑");
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

    public void OnSettingBackBtn()      // 셋팅화면만 나갈 때
    {
        ShowPanel(!is_Setting, settingPanel);
    }

    public void OnGamePauseBackBtn()        // 게임 화면만 나갈 때. backBtn 누르면 실행되는.
    {
        is_GamePause= false;
        gamePausePanel.SetActive(false);
    }

    public void OnSettingShow()     // 게임 멈춤에서 사용하는 것.  인트로에서도 찾아와서 사용해줌.
    {
        ShowPanel(true, settingPanel);
    }

    public void BackBtn()       // 게임 하는 중에 스테이지 선택 창으로 나가려면 = 게임중에 뒤로가기 버튼
    {
        HP.Instance.gameObject.SetActive(false);

        Time.timeScale = 1;
        CloudManager.Instance.Move(true);
        LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");

        GameManager.Instance.nowStageData.BackBtn();

        Debug.Log("UI 랑 뭐시깽스 꺼줘요");
    }
}
