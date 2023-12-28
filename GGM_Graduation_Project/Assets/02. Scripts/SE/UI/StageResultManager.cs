using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultManager : MonoBehaviour
{
    private StageDataSO nowStageData;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject crownPrefab;        // 왕관

    private void Awake()
    {
        nowStageData = GameManager.Instance.nowStageData;
    }

    void Start()
    {
        // 게임매니져의 SO를 가져와서 그걸 기반으로 하여 현제 씬의 여러가지를 설정해준다.
        title.text = nowStageData.stageName;
        slider.value = nowStageData.myPersent;      // 이런거 다 해서 애니? 재생하게 해주기!

        for (int i = 0; i < 3; i++)
        {
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.parent = slider.transform;
            crown.transform.localScale = new Vector2(0, 100);           // x 좌표에 와야 하는 것을 적어줘야함.
        }
    }
}
