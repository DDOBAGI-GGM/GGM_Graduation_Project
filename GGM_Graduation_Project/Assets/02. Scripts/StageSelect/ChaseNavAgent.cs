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
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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

        if (_agent.velocity.sqrMagnitude > 0.2f)
        {
            _animator.SetBool("Move", true);
        }
        else
        {
            _animator.SetBool("Move", false);
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
