using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public IEnumerator Delay(float time)
    {
        for (int i = 0; i < time; i++)
        {
            Debug.Log(i + "ÃÊ Áö³²");
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    public abstract void Interaction(Ingredient type);
}