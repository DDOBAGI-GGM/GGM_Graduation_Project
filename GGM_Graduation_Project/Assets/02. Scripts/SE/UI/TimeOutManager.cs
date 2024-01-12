using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOutManager : MonoBehaviour
{
    [SerializeField] GameObject timeOutPanel;
    [SerializeField] float animTime = 1f;
    public GameObject just;     // 지워도됨.시간 테스트용으로 해둔 것임.

    private void Update()
    {
        just.transform.Translate(new Vector3(-1 * Time.deltaTime, 0, 0));
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
        SettingManager.Instance.FadeAnim(true);
        yield return new WaitForSeconds(0.5f);      // 저 셋팅페이드가 0.5 초라서

        AsyncOperation operation = SceneManager.LoadSceneAsync("StageResult_Scene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //Debug.Log(operation.progress);
            if (operation.progress >= 0.9f)      // 로드가 다 되었을 때
            {
                SettingManager.Instance.FadeAnim(false);
                operation.allowSceneActivation = true;   // 씬 이동
                yield break;
            }
        }
    }
}
