using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOutManager : MonoBehaviour
{
    [SerializeField] GameObject timeOutPanel;
    [SerializeField] float animTime = 1f;
    public GameObject just;     // ��������.�ð� �׽�Ʈ������ �ص� ����.

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
        to = DOTween.To(() => Time.timeScale, scale => Time.timeScale = scale, 0, animTime).OnComplete(()=>Debug.Log("�÷��̾�� ���� �� �����ֱ�!"));
        to.Play();
    }

    private IEnumerator LoadStageResultScene()
    {
        Debug.Log("���̵�");

        yield return new WaitForSeconds(animTime / 10f);     // �÷��̾� ���� ����� /2 �� �ٲپ��ֱ�
        to.Kill();

        Time.timeScale = 1f;
        SettingManager.Instance.FadeAnim(true);
        yield return new WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync("StageResult_Scene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //Debug.Log(operation.progress);
            if (operation.progress >= 0.9f)      // �ε尡 �� �Ǿ��� ��
            {
                SettingManager.Instance.FadeAnim(false);
                operation.allowSceneActivation = true;   
                yield break;
            }
        }
    }
}
