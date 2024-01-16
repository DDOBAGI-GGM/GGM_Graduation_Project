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
    [SerializeField] private Slider slider;
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
            slider.value = nowStageData.starPersent[i];
            GameObject crown = Instantiate(crownPrefab);
            crown.transform.parent = slider.transform;
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
            CloudManager.Instance?.Move(true);
            LoadingSceneManager.Instance?.ChangeLoadScene("StageSelect_Scene");     // �������� ������ �̵��ϱ� �ϸ鼭 ���嵵 ���ְ�?
        }
    }

    private void GageAnim()
    {
        slider.value = nowStageData.myPersent - 0.5f;      // �̷��� �� �ؼ� �ִ�? ����ϰ� ���ֱ�! 50% �� �Ѿ������� �Ǵ��ؼ� �ϱ�!

        // �� ������ �� Ȯ�� �ִϵ� ������ֱ�
        StarInstantiateAnim();
    }

    private void StarInstantiateAnim()
    {
        // ���� �� ���� ���
    }
}
