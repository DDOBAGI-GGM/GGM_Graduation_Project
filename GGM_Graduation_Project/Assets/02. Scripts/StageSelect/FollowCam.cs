using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Ÿ��")][SerializeField] private Transform _trm;
    [Header("������")][SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _trm.position + _offset;
    }
}
