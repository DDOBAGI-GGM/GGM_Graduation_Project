using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageData", fileName = "StageData")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    [Range(0.01f, 1f)]
    public float oneStar, twoStar, threeStar;
    public bool[] star = new bool[3];
    public float myPersent;
}
