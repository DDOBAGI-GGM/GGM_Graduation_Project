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
    [SerializeField] private GameObject crownPrefab;        // �հ�

    private void Awake()
    {
        nowStageData = GameManager.Instance.nowStageData;
    }

    void Start()
    {
        // ���ӸŴ����� SO�� �����ͼ� �װ� ������� �Ͽ� ���� ���� ���������� �������ش�.
        title.text = nowStageData.stageName;
        slider.value = nowStageData.myPersent;      // �̷��� �� �ؼ� �ִ�? ����ϰ� ���ֱ�!

        for (int i = 0; i < 3; i++)
        {
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.parent = slider.transform;
            crown.transform.localScale = new Vector2(0, 100);           // x ��ǥ�� �;� �ϴ� ���� ���������.
        }
    }
}
