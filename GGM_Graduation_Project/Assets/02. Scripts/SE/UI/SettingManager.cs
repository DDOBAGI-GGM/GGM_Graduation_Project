using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject gamePausePanel, settingPanel;       // �����϶� ǥ�õǴ� ����â, ���� â ���� ������.
    [SerializeField] private Animator fadeAnim;     // ���Ʒ��� �����̴� ���̵�

    private bool is_Setting = false;        // ����â�� �����ִ°�
    private bool is_GamePause = false;         // �����߿� ���̴� ����â
    private bool is_Fading = false;     // ���̵� �ִϰ� ����ǰ� �ִ� �� �Ǵ�

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading_Scene" /*&& CloudManager.Instance?.Is_SceneChange == false*/ && is_Fading == false)     
            // �ε����� �ƴҶ�, �ִϸ��̼ǵ� �ȵ��ư� ��, �ּ��� ���� ������ ���̰� ���� ���� �ƴ� ���� ����. ���߿� ���� ���� ���� ��.
        {
            if (SceneManager.GetActiveScene().buildIndex >= 4)       // 4 �̻���� ������������ �־���ٰ� �������� ��
            {
                // ����â�� ���� ���� ���� �� ���Ӽ���â ������ �ϱ�
                if (is_Setting == false)
                {
                    ShowPanel(!is_GamePause, gamePausePanel);
                }
                else
                {
                    // ����â ���ֱ�
                    ShowPanel(!is_Setting, settingPanel);
                }
            }
            else
            {
                // ����â ������ �ϱ�
                ShowPanel(!is_Setting, settingPanel);
            }
            // 2�� �� ������ �ݴ� ���·� �Լ� �����ֱ�.
        }
    }

    public void ShowPanel(bool is_PanelShow, GameObject panel)
    {
        if (is_Fading) return;      // ���̵� ���̸� ����.
        else StartCoroutine(AddFadeAnim(is_PanelShow, panel));
    }

    private IEnumerator AddFadeAnim(bool is_PanelShow, GameObject panel)
    {
        if (panel == gamePausePanel)        // ���� ����â�̶��
        {
            is_GamePause = is_PanelShow;        // ���� â�� ���¸� ��������.
        }
        else if (panel == settingPanel)
        {
            is_Setting = is_PanelShow;
        }

        Time.timeScale = 1;     // �ִϰ� ������ �ϴϱ�. 

        if (is_PanelShow == true)       // �г��� ���̴� �����̸�
        {
            Debug.Log("�÷��̾� ���߰� Ÿ�̸� ���߰� ���ֽʼ�");
        }

        #region ���̰� �Ⱥ��̰�, ���̵� �ִ� �����.
        is_Fading = true;
        FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // �ִϸ��̼� ��� �ð�

        panel.SetActive(is_PanelShow);

        FadeAnim(false);
        yield return new WaitForSeconds(0.5f);
        is_Fading = false;
        #endregion

        if (is_PanelShow == false)      // �г��� ���� ���̸� �ð��� �帣��,
        {
           Debug.Log("�÷��̾� ���߰� Ÿ�̸� ����� Ǯ���ֽʼ�");
        }
        else
        {
            Time.timeScale = 0;     // ����â�̵� ���� â�� ���� ���̸�
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

    public void OnSettingBackBtn()      // ����ȭ�鸸 ���� ��
    {
        ShowPanel(!is_Setting, settingPanel);
    }

    public void OnGamePauseBackBtn()        // ���� ȭ�鸸 ���� ��. backBtn ������ ����Ǵ�.
    {
        is_GamePause= false;
        gamePausePanel.SetActive(false);
    }

    public void OnSettingShow()     // ���� ���㿡�� ����ϴ� ��.  ��Ʈ�ο����� ã�ƿͼ� �������.
    {
        ShowPanel(true, settingPanel);
    }

    public void BackBtn()       // ���� �ϴ� �߿� �������� ���� â���� �������� = �����߿� �ڷΰ��� ��ư
    {
        HP.Instance.gameObject.SetActive(false);

        Time.timeScale = 1;
        CloudManager.Instance.Move(true);
        LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");

        GameManager.Instance.nowStageData.BackBtn();

        Debug.Log("UI �� ���ò��� �����");
    }
}
