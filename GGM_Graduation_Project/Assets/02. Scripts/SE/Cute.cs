using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cute : MonoBehaviour
{
    public float deley = 0.3f;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(deley);
        GetComponent<Animator>().enabled = true;
    }
}
