using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum test
{
    test1,
    test2, test3
}

public class testcode : MonoBehaviour
{
    public string go;
    static public bool check = false;
    public StageDataSO so;

    private void Start()
    {
        Debug.Log(check);
    }

    test a = test.test1;

    public int Test() => a switch
    {
        test.test1 => 1,
        test.test2 => 2,
        _ => 3,
    };

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