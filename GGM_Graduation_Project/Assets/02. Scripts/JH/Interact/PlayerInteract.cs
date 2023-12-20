using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private InteractUI interactUI;

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
    private void Awake()
    {
        interactUI = GetComponent<InteractUI>();
    }

    private void Update()
    {
        interactUI.UpdateText(string.Empty);
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                interactUI.UpdateText(interactable.promptMessage);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}