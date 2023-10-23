using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SINGLETON<T> : MonoBehaviour where T : Component
{
    private static T instnace;

    public static T Instance
    {
        get
        {
            if (instnace == null)
            {
                instnace = FindObjectOfType<T>();
                if (instnace == null)
                {
                    GameObject newInstance = new GameObject();
                    instnace = newInstance.AddComponent<T>();
                }
            }

            return instnace;
        }
    }
}
