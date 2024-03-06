using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSOManager : MonoBehaviour
{
    [SerializeField] private List<StageDataSO> stageData;

    private void Awake()
    {
        SaveAndLoadManager.Instance.Load();
        for (int i = 0; i < SaveAndLoadManager.Instance.saveData.stagePersentData.Count; i++)
        {
            stageData[i].myPersent = SaveAndLoadManager.Instance.saveData.stagePersentData[i];
            stageData[i].PersentSetting();
        }
    }
}