using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTag : MonoBehaviour
{
    private Camera cam;
    public Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            return cam;
        }
    }

    Vector3 startScale;
    public float distance = 3;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;

        transform.rotation = Cam.transform.rotation;
    }
}
