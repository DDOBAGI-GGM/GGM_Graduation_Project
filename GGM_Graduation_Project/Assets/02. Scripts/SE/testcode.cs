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

    private void dd()
    {
      /*  float a = 0;
        a = Time.time;
        while (true)
        {
            Debug.Log(a);
            if (a  - Time.time < -5)
            {
                break;
            }
        }*/
    }
}