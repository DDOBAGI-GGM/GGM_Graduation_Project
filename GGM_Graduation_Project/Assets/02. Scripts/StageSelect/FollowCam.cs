using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Ÿ��")][SerializeField] private Transform _trm;
    [Header("������")][SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        transform.position = _trm.position + _offset;
    }
}
