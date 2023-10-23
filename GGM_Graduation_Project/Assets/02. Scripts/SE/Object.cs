using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public abstract void Interaction();
}