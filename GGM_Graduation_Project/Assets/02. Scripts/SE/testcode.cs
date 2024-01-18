using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class testcode : MonoBehaviour
{
    public string go;
    static public bool check = false;
    public StageDataSO so;

    private void Start()
    {
        Debug.Log(check);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(go);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            check = true;
        }

        if (Input.GetKeyDown (KeyCode.E))
        {
            GameManager.Instance.nowStageData.myPersent = 0.1f;
        }
    }
}