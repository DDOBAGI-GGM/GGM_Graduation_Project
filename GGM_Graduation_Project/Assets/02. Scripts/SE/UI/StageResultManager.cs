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
    [SerializeField] private GameObject crownPrefab;        // �հ�

    private void Awake()
    {
        nowStageData = GameManager.Instance.nowStageData;
    }

    void Start()
    {
        // ���ӸŴ����� SO�� �����ͼ� �װ� ������� �Ͽ� ���� ���� ���������� �������ش�.
        title.text = nowStageData.stageName;

        for (int i = 0; i < 3; i++)
        {
            resultSlider.value = nowStageData.starPersent[i] * 2 - 1;         // ���� ��ġ�� �����ֱ� ���ؼ���. �̰� ��� ���ҿ�
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.SetParent(resultSlider.transform);
            crown.transform.position = new Vector2(sliderGagePos.transform.position.x, 0);           // x ��ǥ ����
            crown.transform.localPosition = new Vector2(crown.transform.localPosition.x, 100);          // y ��ǥ ����
        }

        // �Լ� �ҷ��ֱ� ���� ���� �� ����ؼ� �־ �������ֱ�!!


        GageAnim();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))       // Enter �� ������ �ڷ� ����.
        {
            Debug.Log("�������� ���� ������ �̵��ϱ�");
            CloudManager.Instance.Move(true);
            LoadingSceneManager.Instance.ChangeLoadScene("StageSelect_Scene");     // �������� ������ �̵��ϱ� �ϸ鼭 ���嵵 ���ְ�?
        }
    }

    private void GageAnim()
    {
        resultSlider.value = nowStageData.myPersent - 0.5f > 0 ? nowStageData.myPersent - 0.5f : 0 ;      // �̷��� �� �ؼ� �ִ�? ����ϰ� ���ֱ�! 50% �� �Ѿ������� �Ǵ��ؼ� �ϱ�!

        // �� ������ �� ȹ�� �ִϵ� ������ֱ�
        StarInstantiateAnim();
    }

    private void StarInstantiateAnim()
    {
        // ���� �� ���� ���
    }
}
