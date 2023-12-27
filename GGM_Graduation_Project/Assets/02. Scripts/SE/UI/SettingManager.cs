using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Animator fadeAnim;

     private bool is_NowShow = false;
     private bool is_fading = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Loading_Scene" && CloudManager.Instance?.Is_SceneChange == false)      // �ε����� �ƴҶ�, �ִϸ��̼ǵ� �ȵ��ư� ��
        {
            if (SceneManager.GetActiveScene().buildIndex > 5)       // ���� 5 �̻���� ������������ �־���ٰ� �������� ��
            {
                // �ΰ��Ӽ���â 1? ������ �ϱ�
            }
            else
            {
                // �ٷ� ����â ������!
                if (!is_fading)
                {
                    ShowSetting(!is_NowShow);
                }
            }
        }
    }

    public void ShowSetting(bool is_PanelShow)
    {
        if (is_fading) return;
        else StartCoroutine(AddFadeAnim(is_PanelShow));
    }

    private IEnumerator AddFadeAnim(bool is_PanelShow)
    {
        if (is_PanelShow) is_NowShow = true;
        is_fading = true;
        fadeAnim.SetTrigger("In");
        yield return new WaitForSeconds(0.5f);
        settingPanel.gameObject.SetActive(is_PanelShow);
        fadeAnim.SetTrigger("Out");
        yield return new WaitForSeconds(0.5f);
        is_fading = false;
        if (!is_PanelShow) is_NowShow = false;
    }
}
