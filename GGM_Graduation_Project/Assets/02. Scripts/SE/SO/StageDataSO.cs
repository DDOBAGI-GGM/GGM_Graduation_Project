using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;        // �������� �̸�
    [Range(50f, 200f)] public float[] starPersent = new float[3];        // ���� �־�� �ϴ� ��
    public bool[] star = new bool[3];       // �� ����
    [Range(0f, 200f)] public float myPersent;     // �� �ۼ�Ʈ
    public float gameTime;      // Ÿ�ӿ��� �ð�

    public void PersentSetting()      // �ۼ�Ʈ�� ���� ������ ���ִ� ��.
    {
        Debug.Log(myPersent);
        for (int i = 0; i < 3; i++)
        {
            if (myPersent >= starPersent[i])     // �� �ۼ�Ʈ�� �� ��� �ۼ�Ʈ���� ũ�ų� ������
            {
                star[i] = true;
            }
            else
            {
                star[i] = false;
            }
        }
    }

    public void BackBtn()
    {
        myPersent = 0;
        PersentSetting();
    }
}
