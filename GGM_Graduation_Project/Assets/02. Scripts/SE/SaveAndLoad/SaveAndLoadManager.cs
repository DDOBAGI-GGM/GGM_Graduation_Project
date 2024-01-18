using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<int> stagePersentData = new List<int>();
}

public class SaveAndLoadManager : Singleton<SaveAndLoadManager>
{
    public SaveData saveData = new SaveData();     // 저장될 클래스
    private string savePath;        // 저장될 데이더 경로

    private void Start()
    {
        savePath = Application.dataPath + "/SaveData";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    [ContextMenu("저장")]
    public void Save()
    {
        Debug.Log("Save");
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath + "/SaveFile.txt", json);
    }

    [ContextMenu("불러오기")]
    public void Load()
    {
        if (File.Exists(savePath + "/SaveFile.txt"))
        {
            Debug.Log("Load");
            string LoadJson = File.ReadAllText(savePath + "/SaveFile.txt");
            saveData = JsonUtility.FromJson<SaveData>(LoadJson);
        }
        else Debug.Log("저장 파일 없음");
    }
}


/*
 파일 존재 유무 판단은
if (File.Exists(Application.dataPath + "/SaveData/SaveFile.txt")) {}
 이렇게 해서 확인함.
 */