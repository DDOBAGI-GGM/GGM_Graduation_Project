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
    public SaveData saveData = new SaveData();     // ����� Ŭ����
    private string savePath;        // ����� ���̴� ���

    private void Start()
    {
        savePath = Application.dataPath + "/SaveData";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    [ContextMenu("����")]
    public void Save()
    {
        Debug.Log("Save");
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath + "/SaveFile.txt", json);
    }

    [ContextMenu("�ҷ�����")]
    public void Load()
    {
        if (File.Exists(savePath + "/SaveFile.txt"))
        {
            Debug.Log("Load");
            string LoadJson = File.ReadAllText(savePath + "/SaveFile.txt");
            saveData = JsonUtility.FromJson<SaveData>(LoadJson);
        }
        else Debug.Log("���� ���� ����");
    }
}


/*
 ���� ���� ���� �Ǵ���
if (File.Exists(Application.dataPath + "/SaveData/SaveFile.txt")) {}
 �̷��� �ؼ� Ȯ����.
 */