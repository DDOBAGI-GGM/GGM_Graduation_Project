using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField] protected Transform _targetTrm;
    public Transform Target => _targetTrm;

    public NavMeshAgent NavAgent { get; private set; }

    //protected UIStatusBar _statueBar;
    protected Camera _mainCam;

    public NodeActionCode currentCode;

    protected virtual void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        //_statueBar = transform.Find("Status").GetComponent<UIStatusBar>();
        _mainCam = Camera.main;
    }

    protected void LateUpdate()
    {
        //_statueBar.LookToCamera(); //이건 update에서 호출하면 지터링(계단현상) 발생
    }

    private Coroutine _coroutine;
    /*public void TryToTalk(string text, float timer = 1)
    {
        _statueBar.DialogText = text;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(PanelFade(timer));
    }*/

    /*IEnumerator PanelFade(float timer)
    {
        _statueBar.IsOn = true;
        yield return new WaitForSeconds(timer);
        _statueBar.IsOn = false;
    }*/

    public abstract void Attack();
}
