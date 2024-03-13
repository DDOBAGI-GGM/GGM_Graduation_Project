using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOutManager : Singleton<TimeOutManager>
{
    [SerializeField] GameObject timeOutPanel;
    [SerializeField] float animTime = 1f;
    //public GameObject just;     // 지워도됨.시간 테스트용으로 해둔 것임.

    private void Update()
    {
       // just.transform.Translate(new Vector3(-1 * Time.deltaTime, 0, 0));
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeOutShow();
        }
    }

    Tween to;
    public void timeOutShow()
    {
        timeOutPanel.transform.DOScale(new Vector3(1, 1, 1), animTime / 2).SetEase(Ease.OutBack).OnComplete(() => StartCoroutine(LoadStageResultScene()));
        to = DOTween.To(() => Time.timeScale, scale => Time.timeScale = scale, 0, animTime);
        to.Play();
    }

    private IEnumerator LoadStageResultScene()
    {

        yield return new WaitForSeconds(animTime / 10f);     // 플레이어 멈춤 만들면 /2 로 바꾸어주기
        // 플레이어의 이동 멈춰주기
        Debug.Log("플레이어 이동 멈춰주십쇼");
       to.Kill();

        Time.timeScale = 1f;

        HP.Instance.Gage.gameObject.SetActive(false);
        GameManager.Instance.nowStageData.myPersent = HP.Instance.Gage.value;
        GameManager.Instance.nowStageData.PersentSetting();
        CloudManager.Instance?.Move(true);
        LoadingSceneManager.Instance?.ChangeLoadScene("StageResult_Scene");

        yield return new WaitForSeconds(2f);
        timeOutPanel.transform.localScale = Vector3.zero;
    }
}
