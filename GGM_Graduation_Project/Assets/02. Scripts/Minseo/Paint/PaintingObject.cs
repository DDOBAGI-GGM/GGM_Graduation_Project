using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingObject : MonoBehaviour
{
    [SerializeField] private Color paintColor;

    [SerializeField] private float minRadius = 0.05f;
    [SerializeField] private float maxRadius = 0.2f;
    [SerializeField] private float strength = 1;
    [SerializeField] private float hardness = 1;

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    if (collision.gameObject.CompareTag("Plane"))
    //    {
    //        Debug.Log("����   ");

    //        Paintable paintableObjects = collision.transform.gameObject.GetComponent<Paintable>();

    //        if (paintableObjects == null || paintableObjects.Length == 0) return;

    //        Vector3 point = collision.contacts[0].point;

    //        foreach (Paintable obj in paintableObjects)
    //        {
    //            PaintManager.Instance.Paint(paintableObjects, point, Random.Range(minRadius, maxRadius), hardness, strength, paintColor);
    //        }
    //        Debug.Log("��");
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Paintable p = collision.collider.GetComponent<Paintable>();
        if (p != null)
        {
            Vector3 pos = collision.contacts[0].point;
            PaintManager.Instance.Paint(p, pos, Random.Range(minRadius, maxRadius), hardness, strength, paintColor);
        }
    }
}
