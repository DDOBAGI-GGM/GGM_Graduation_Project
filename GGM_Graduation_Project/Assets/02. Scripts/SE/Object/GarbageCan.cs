using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class GarbageCan : MonoBehaviour, IObject
{
    public GameObject Interaction(GameObject ingredient)
    {
        //Ǯ�� ����ؼ� ���ֱ�
        if (ingredient != null)
        {
            SoundManager.Instance.PlaySFX("get");
            Debug.Log("�����ֱ�");
            Destroy(ingredient);
        }
        return null;
    }
}
