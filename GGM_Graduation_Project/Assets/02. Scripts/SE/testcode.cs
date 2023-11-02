using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour
{
    public static testcode instance;

    public static testcode Instance { get { return instance; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dd();
            Debug.Log("tlqk");
        }
    }

    GameObject a;

    private void dd()
    {
     /*   var scripts = a.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().IsSubclassOf(typeof(Ingredient)))
            {
                // 원하는 처리를 수행합니다.
            }
        }*/

    }
}