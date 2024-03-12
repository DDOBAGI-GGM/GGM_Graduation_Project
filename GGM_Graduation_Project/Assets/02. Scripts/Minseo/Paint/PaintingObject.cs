using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PaintingObject : MonoBehaviour
{
    [SerializeField] private GameObject paintObj;
    [SerializeField] private bool isPlayer = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PaintPlane"))
        {
            /* ����Ʈ �� �ڵ�
             * Paintable p = collision.collider.GetComponent<Paintable>();
            if (p != null)
            {
                Vector3 pos = collision.contacts[0].point;
                PaintManager.Instance.Paint(p, pos, Random.Range(minRadius, maxRadius), hardness, strength, paintColor);
            }*/

            Vector3 pos = collision.contacts[0].point;

            Instantiate(paintObj, pos, Quaternion.identity);
            HP.Instance.SetValue(isPlayer);

            Destroy(gameObject);
        }
    }
    //[SerializeField] private float minRadius = 0.05f;
    //[SerializeField] private float maxRadius = 0.2f;
    //[SerializeField] private float strength = 1;
    //[SerializeField] private float hardness = 1;

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    if (collision.gameObject.CompareTag("Plane"))
    //    {
    //        Debug.Log("����   ");

    //        Paintable[] paintableObjects = collision.transform.GetComponentsInChildren<Paintable>();

        //if (paintableObjects == null || paintableObjects.Length == 0) return;

        //Vector3 point = collision.contacts[0].point;

        //foreach (Paintable obj in paintableObjects)
        //{
        //    GameManager.Instance.PaintManager.Paint(obj, point, Random.Range(0.5f, 0.9f), 1, 1, color);
        //}
//        Debug.Log("��");
//    }
//}
}
