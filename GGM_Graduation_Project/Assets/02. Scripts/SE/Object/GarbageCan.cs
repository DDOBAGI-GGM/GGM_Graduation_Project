using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : MonoBehaviour, IObject
{
    public void Interaction(GameObject ingredient)
    {
        //Ǯ�� ����ؼ� ���ֱ�
        Destroy(ingredient);
    }
}
