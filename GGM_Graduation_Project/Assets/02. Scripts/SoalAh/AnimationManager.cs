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
    IDLE, // 정지 상태
    WALK, // 이동 상태
    CARRY, // 운반 상태
    PERFORM, // 작업 상태
    TRASH, // 쓰레기 사용
    ATTACK, // 공격 상태
    HIT, // 피해 상태
    DIE // 사망 상태
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
    
    // 함수를 하나로 할까 뽀갤까...
}
