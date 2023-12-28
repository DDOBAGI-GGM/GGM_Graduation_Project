using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] protected float maxLifetime, stickTime;
    [SerializeField] protected Material paintMaterial;

    private void Start()
    {
        Invoke("DestroySelf", maxLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StickToSurface(collision.contacts[0].point);
        Invoke("DestroySelf", stickTime);
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void StickToSurface(Vector3 hitPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(hitPoint + Vector3.up * 0.1f, Vector3.down, out hit, 1.0f))
        {
            // Check if the hit object has a renderer
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Apply the paint material to the hit point
                renderer.material = paintMaterial;
            }
        }
    }

    private void DestroySelf()
    {
        if (this)
        {
            Destroy(gameObject);
        }
    }
}
