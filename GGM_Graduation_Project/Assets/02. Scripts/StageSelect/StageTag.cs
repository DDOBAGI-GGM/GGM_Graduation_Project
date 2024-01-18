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