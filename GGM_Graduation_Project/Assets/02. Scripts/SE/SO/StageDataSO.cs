using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;        // 스테이지 이름
    [Range(50f, 200f)] public float[] starPersent = new float[3];        // 별이 있어야 하는 것
    public bool[] star = new bool[3];       // 별 개수
    [Range(0f, 200f)] public float myPersent;     // 내 퍼센트
    public float gameTime;      // 타임오버 시간

    public void PersentSetting()      // 퍼센트에 따라서 리셋을 해주는 것.
    {
        Debug.Log(myPersent);
        for (int i = 0; i < 3; i++)
        {
            if (myPersent >= starPersent[i])     // 내 퍼센트가 별 얻는 퍼센트보다 크거나 같으면
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
