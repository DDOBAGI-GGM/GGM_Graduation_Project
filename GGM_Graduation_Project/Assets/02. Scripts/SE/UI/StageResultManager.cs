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

        for (int i = 0; i < 3; i++)
        {
            resultSlider.value = nowStageData.starPersent[i] * 2 - 1;         // 별의 위치를 정해주기 위해서임. 이걸 어떻게 하죠웅
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.SetParent(resultSlider.transform);
            crown.transform.position = new Vector2(sliderGagePos.transform.position.x, 0);           // x 좌표 설정
            crown.transform.localPosition = new Vector2(crown.transform.localPosition.x, 100);          // y 좌표 설정
        }

        // 함수 불러주기 전에 값들 다 계산해서 넣어서 저장해주기!!


        GageAnim();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))       // Enter 를 누르면 뒤로 가게.
        {
            Debug.Log("스테이지 선택 씬으로 이동하기");
            CloudManager.Instance.Move(true);
            LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");     // 스테이지 씬으로 이동하기 하면서 저장도 해주고?
        }
    }

    private void GageAnim()
    {
        resultSlider.value = nowStageData.myPersent - 0.5f > 0 ? nowStageData.myPersent - 0.5f : 0 ;      // 이런거 다 해서 애니? 재생하게 해주기! 50% 를 넘었는지도 판단해서 하기!

        // 다 끝나면 별 획득 애니도 재생해주기
        StarInstantiateAnim();
    }

    private void StarInstantiateAnim()
    {
        // 얻은 별 개수 출력
    }
}
