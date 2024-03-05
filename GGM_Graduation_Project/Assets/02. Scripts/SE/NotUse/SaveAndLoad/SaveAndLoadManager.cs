using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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
        saveData.stagePersentData.Add(1);
    }

    [ContextMenu("저장")]
    public void Save()
    {
        Debug.Log("Save");
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath + "/SaveFile.txt", json);
        AssetDatabase.Refresh();        // 이건 후에 삭제해주기, 저 유니티 에디터와 같이
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

    [ContextMenu("파일 지우기")]
    public void DeleteSaveData()
    {
        if (File.Exists(savePath + "/SaveFile.txt"))
        {
            File.Delete(savePath + "/SaveFile.txt.meta");
            File.Delete(savePath + "/SaveFile.txt");
            AssetDatabase.Refresh();
        }
        else Debug.Log("저장 파일 없음");
    }
}


/*
 파일 존재 유무 판단은
if (File.Exists(Application.dataPath + "/SaveData/SaveFile.txt")) {}
 이렇게 해서 확인함.
 */