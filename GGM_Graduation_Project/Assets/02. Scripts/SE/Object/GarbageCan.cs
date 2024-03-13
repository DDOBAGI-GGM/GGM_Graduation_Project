using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class GarbageCan : MonoBehaviour, IObject
{
    public GameObject Interaction(GameObject ingredient)
    {
        if (ingredient != null)
        {
            SoundManager.Instance.PlaySFX("get");
            Destroy(ingredient);
        }
        return null;
    }
}
