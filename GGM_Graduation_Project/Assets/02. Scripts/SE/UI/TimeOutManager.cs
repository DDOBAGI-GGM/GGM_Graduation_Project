using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOutManager : MonoBehaviour
{
    public static TimeOutManager instance;

    [SerializeField] GameObject timeOutPanel;
    [SerializeField] float animTime = 1f;

    private void Awake()
    {
        instance = this;
    }

    Tween to;
    public void TimeOutShow()
    {
        timeOutPanel.transform.DOScale(new Vector3(1, 1, 1), animTime / 2).SetEase(Ease.OutBack).OnComplete(() => StartCoroutine(LoadStageResultScene()));
        to = DOTween.To(() => Time.timeScale, scale => Time.timeScale = scale, 0, animTime);
        to.Play();
    }

    private IEnumerator LoadStageResultScene()
    {

        yield return new WaitForSeconds(animTime / 10f);     // �÷��̾� ���� ����� /2 �� �ٲپ��ֱ�
        // �÷��̾��� �̵� �����ֱ�
        Debug.Log("�÷��̾� �̵� �����ְ� �ۼ�Ʈ �ٽ� ����");
        HP.Instance.Gage.value = GameManager.Instance.nowStageData.myPersent;
        GameManager.Instance.nowStageData.PersentSetting();
       to.Kill();

        Time.timeScale = 1f;
        SettingManager.Instance?.FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // �� �������̵尡 0.5 �ʶ�

        AsyncOperation operation = SceneManager.LoadSceneAsync("StageResult_Scene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)      // �ε尡 �� �Ǿ��� ��
            {
                SettingManager.Instance?.FadeAnim(false);
                operation.allowSceneActivation = true;   // �� �̵�
                yield break;
            }
        }
    }
}
