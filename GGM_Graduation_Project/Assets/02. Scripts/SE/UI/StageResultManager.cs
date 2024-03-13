using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultManager : MonoBehaviour
{
    private StageDataSO nowStageData;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Slider resultSlider;
    [SerializeField] private GameObject sliderGagePos;
    [SerializeField] private GameObject crownPrefab;        // 왕관

    private void Awake()
    {
        nowStageData = GameManager.Instance.nowStageData;
    }

    void Start()
    {
        // 게임매니져의 SO를 가져와서 그걸 기반으로 하여 현제 씬의 여러가지를 설정해준다.
        title.text = nowStageData.stageName;
        Debug.Log(nowStageData.myPersent);

        for (int i = 0; i < 3; i++)
        {
            resultSlider.value = nowStageData.starPersent[i];         // 별의 위치를 정해주기 위해서임. 이걸 어떻게 하죠웅
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.SetParent(resultSlider.transform);
            crown.transform.position = new Vector2(sliderGagePos.transform.position.x, 0);           // x 좌표 설정
            crown.transform.localPosition = new Vector2(crown.transform.localPosition.x, 100);          // y 좌표 설정
        }

        GageAnim();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))       // Enter 를 누르면 뒤로 가게.
        {
            Debug.Log("스테이지 선택 씬으로 이동하기");
            CloudManager.Instance.Move(true);
            LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");     // 스테이지 씬으로 이동하기 하면서 저장도 해주게 만들기
        }
    }

    private void GageAnim()
    {
        resultSlider.value = 0;
        Tween to = DOTween.To(() => resultSlider.value, value => resultSlider.value = value, nowStageData.myPersent, 1).SetEase(Ease.OutCubic);
    }
}
