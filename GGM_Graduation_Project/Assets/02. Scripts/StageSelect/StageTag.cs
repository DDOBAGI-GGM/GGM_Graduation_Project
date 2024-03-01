using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTag : MonoBehaviour
{
    private Camera cam;

    Vector3 startScale;
    public float distance = 3;

    private void Start()
    {
        startScale = transform.localScale;
        cam = Camera.main;
    }

    private void Update()
    {
        float dist = Vector3.Distance(cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;

        transform.rotation = cam.transform.rotation;
    }
}

// 이거 코드 안 쓸 뜻? 플레이어가 자기에게서 멀어지면 나의 크키가 커지게 하는 코드임.
// 오버쿡트에 들어가있긴 함.