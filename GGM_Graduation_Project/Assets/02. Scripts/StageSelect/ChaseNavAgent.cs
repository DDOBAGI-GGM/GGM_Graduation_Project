using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNavAgent : MonoBehaviour
{
    [SerializeField]
    private LayerMask _whatIsBase;

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

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos;
            if (GetMouseWorldPosition(out pos))
            {
                _agent.SetDestination(pos);
            }
        }
    }

    public bool GetMouseWorldPosition(out Vector3 pos)
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool result = Physics.Raycast(ray, out hit, Cam.farClipPlane, _whatIsBase);
        if (result)
        {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
