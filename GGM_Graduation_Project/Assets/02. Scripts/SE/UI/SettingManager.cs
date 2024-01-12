using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject gamePausePanel, settingPanel;
    [SerializeField] private Animator fadeAnim;

    private bool is_NowShow = false;
    private bool is_Fading = false;     // ���̵� �ִϰ� ����ǰ� �ִ� �� �Ǵܤ�

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading_Scene" /*&& CloudManager.Instance?.Is_SceneChange == false*/ && is_Fading == false)      // �ε����� �ƴҶ�, �ִϸ��̼ǵ� �ȵ��ư� ��, �ּ��� �׽�Ʈ ������ ����
        {
            if (SceneManager.GetActiveScene().buildIndex >= 4)       // ���� 4 �̻���� ������������ �־���ٰ� �������� ��
            {
                // �ΰ��Ӽ���â 1? ������ �ϱ�
                ShowPanel(!is_NowShow, gamePausePanel);
            }
            else
            {
                Debug.Log("����!");
                ShowPanel(!is_NowShow, settingPanel);
                // �ٷ� ����â ������!
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
        //Debug.Log("��");
        if (is_PanelShow)
        {
            is_NowShow = true;
        }

        Time.timeScale = 1;     // �ִϰ� ������ �ϴϱ�.

        is_Fading = true;
        FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // �ִϸ��̼� ��� �ð�

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
            Time.timeScale = 0;     // ����â�� �����Ÿ�
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
