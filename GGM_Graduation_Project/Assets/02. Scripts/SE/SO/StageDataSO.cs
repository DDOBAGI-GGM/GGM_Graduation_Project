using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;        // �������� �̸�
    [Range(0.5f, 1f)] public float[] starPersent = new float[3];        // ���� �־�� �ϴ� ��
    public bool[] star = new bool[3];       // �� ����
    [Range(0f, 1f)] public float myPersent;     // �� �ۼ�Ʈ
    public float gameTime;      // Ÿ�ӿ��� �ð�

    public void PersentReset()      // �ۼ�Ʈ�� ���� ������ ���ִ� ��.
    {
        for (int i = 0; i < 3; i++)
        {
            if (myPersent >= starPersent[i])     // �� �ۼ�Ʈ�� �� ��� �ۼ�Ʈ���� ũ�ų� ������
            {
                star[i] = true;
            }
        }
    }

    public StageDataSO CopySO(StageDataSO copy)        // Ŀ���ϴ� �Ϳ� �������� �ִ� �����͸� �����ؼ� �־���.
    {
        copy.stageName = this.stageName;
        copy.starPersent = this.starPersent;
        copy.star = this.star;
        copy.myPersent = this.myPersent;
        copy.gameTime = this.gameTime;
        return copy;
    }
}
