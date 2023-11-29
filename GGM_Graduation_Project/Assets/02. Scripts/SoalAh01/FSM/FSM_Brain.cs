using BehaviorTree;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using UnityEditor.Animations.Rigging;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FSM_Brain : MonoBehaviour
{
    NavMeshAgent _agent;

    [Header("State")]
    [SerializeField] FSM_State curState; // ���� ����
    [SerializeField] List<FSM_State> states = new List<FSM_State>(); // ���� ����Ʈ
    public List<GameObject> devicePos = new List<GameObject>(); // ������Ʈ ����Ʈ

    [Header("Hand")]
    public GameObject _hand = null; // ���� AI�� ��� �ִ� ������
    public ObjectType _curObjType = ObjectType.NONE; // �տ� ����ִ� ������ Ÿ��
    public GameObject destination;

    public Dictionary<string, IObject> interaction_Dic = new Dictionary<string, IObject>();

    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();

        // �� ������Ʈ�� �ִ� ����� ����ϱ� ���� Iobject�� �޾ƿ��� ������ �� �޾ƿ´�...
        for (int i = 0; i < devicePos.Count; ++i)
        {
            Debug.Log(i);
            IObject obj = devicePos[i].GetComponent<IObject>();
            if (obj != null)
            {
                interaction_Dic.Add(devicePos[i].name, obj);
                Debug.Log(interaction_Dic[devicePos[i].name]);
            }
            //interaction_Dic.Add(devicePos[i].name, obj);
        }
    }

    public void Reset()
    {
        curState = null;
        destination = null;
    }

    private void Update()
    {
        // ���� �˻�
        foreach (FSM_State state in states)
            state.CheckTransition();

        // �ൿ ����
        curState?.PlayAction(destination);

        // �� ����...
        hand();
    }

    public void ChangeState(FSM_State _state)
    {
        // ���� ��ȯ
        Debug.Log(_state.name);
        curState = _state;
    }

    public bool SetDestination(GameObject _targets)
    {
        Debug.Log("�̵�");
        return true;
        // ������ ������ �ݺ�
        //while (true)
        //{
        //    // �����ߴٸ�
        //    if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f) 
        //        return true;
        //}
    }

    private void hand()
    {
        // �տ� � Ÿ���� ������Ʈ�� �ִ���

        // �տ� �ƹ��͵� ������� �ʴٸ� none
        if (_hand.transform.childCount == 0)
            _curObjType = ObjectType.NONE;
        // �տ� ���𰡸� ����ִٸ� �� ������Ʈ�� recipe�� ���� ������Ʈ Ÿ�� �޾ƿ���
        else
            _curObjType = _hand.transform.GetChild(0).GetComponent<RecipeSO>().objType;
    }
}
