using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneRemove : MonoBehaviour
{
    private void Awake()
    {
        gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf('('));
    }
}
