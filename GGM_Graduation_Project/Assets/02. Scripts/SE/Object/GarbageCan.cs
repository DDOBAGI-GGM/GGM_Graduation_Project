using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : MonoBehaviour, IObject
{
    public void Interaction(GameObject ingredient)
    {
        //풀링 사용해서 해주기
        Destroy(ingredient);
    }
}
