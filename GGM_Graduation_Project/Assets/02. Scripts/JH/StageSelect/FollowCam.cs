using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Å¸°Ù")][SerializeField] private Transform _trm;
    [Header("¿ÀÇÁ¼Â")][SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _trm.position + _offset;
    }
}
