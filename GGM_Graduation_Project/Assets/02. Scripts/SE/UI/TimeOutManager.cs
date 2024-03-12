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

        yield return new WaitForSeconds(animTime / 10f);     // 플레이어 멈춤 만들면 /2 로 바꾸어주기
        // 플레이어의 이동 멈춰주기
        Debug.Log("플레이어 이동 멈춰주고 퍼센트 다시 셋팅");
        HP.Instance.Gage.value = GameManager.Instance.nowStageData.myPersent;
        GameManager.Instance.nowStageData.PersentSetting();
       to.Kill();

        Time.timeScale = 1f;
        SettingManager.Instance?.FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // 저 셋팅페이드가 0.5 초라서

        AsyncOperation operation = SceneManager.LoadSceneAsync("StageResult_Scene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)      // 로드가 다 되었을 때
            {
                SettingManager.Instance?.FadeAnim(false);
                operation.allowSceneActivation = true;   // 씬 이동
                yield break;
            }
        }
    }
}
