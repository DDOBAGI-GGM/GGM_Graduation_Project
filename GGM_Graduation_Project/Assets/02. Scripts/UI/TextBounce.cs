using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBounce : MonoBehaviour
{
    [Header("커질 크기")][SerializeField] private float _size;
    [Header("진행 시간")][SerializeField] private float _upSizeTime;
    private float _time = 0;

    private void Update()
    {
        if (_time <= _upSizeTime)
        {
            transform.localScale = Vector3.one * (1 + _size * _time);
        }
        else if (_time <= _upSizeTime * 2)
        {
            transform.localScale = Vector3.one * (2 * _size * _upSizeTime + 1 - _time * _size);
        }
        else
        {
            _time = 0;
            transform.localScale = Vector3.one;
        }
        _time += Time.deltaTime;
    }
}

// 이거 코드 너무 시끄러움. -성은-