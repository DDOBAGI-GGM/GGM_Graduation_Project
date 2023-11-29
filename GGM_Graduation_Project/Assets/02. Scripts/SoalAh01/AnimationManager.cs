using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharType
{
    PLAYER,
    AI
}

public enum AnimType
{
    IDLE, // ���� ����
    WALK, // �̵� ����
    CARRY, // ��� ����
    PERFORM, // �۾� ����
    TRASH, // ������ ���
    ATTACK, // ���� ����
    HIT, // ���� ����
    DIE // ��� ����
}

public class AnimationManager : SINGLETON<AnimationManager>
{
    [SerializeField] private List<Animator> _animators = new List<Animator>();
    public void PlayAnim(CharType _charType, AnimType _animType, string _name)
    {
        switch (_animType)
        {
            case AnimType.IDLE:
                _animators[(int)_charType].GetBool(_name);
                break;
            case AnimType.WALK:
                _animators[(int)_charType].GetBool(_name);
                break;
            // ...
            default:
                Debug.Log("AnimMgr::Error::switchLoop");
                break;
        }
    }
    
    // �Լ��� �ϳ��� �ұ� �ǰ���...
}
