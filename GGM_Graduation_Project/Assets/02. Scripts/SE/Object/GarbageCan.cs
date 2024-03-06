using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class GarbageCan : MonoBehaviour, IObject
{
    public GameObject Interaction(GameObject ingredient)
    {
        //풀링 사용해서 해주기
        if (ingredient != null)
        {
            SoundManager.Instance.PlaySFX("get");
            Debug.Log("버려주기");
            Destroy(ingredient);
        }
        return null;
    }
}
