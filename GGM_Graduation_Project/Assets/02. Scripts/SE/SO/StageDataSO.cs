using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    [Range(0.5f, 1f)]
    public float[] starPersent = new float[3]; 
    public bool[] star = new bool[3];
    [Range(0f, 1f)]
    public float myPersent;
}
