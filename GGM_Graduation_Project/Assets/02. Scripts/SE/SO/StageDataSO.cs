using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    [Range(0.5f, 1f)] public float[] starPersent = new float[3]; 
    public bool[] star = new bool[3];
    [Range(0f, 1f)] public float myPersent;

    public void PersentReset()      // 퍼센트에 따라서 리셋을 해주는 것.
    {
        for (int i = 0; i < 3; i++)
        {
            if (myPersent >= starPersent[i])     // 내 퍼센트가 별 얻는 퍼센트보다 크거나 같으면
            {
                star[i] = true;
            }
        }
    }
}
