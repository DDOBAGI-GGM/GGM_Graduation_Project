using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour       // 그냥 실글턴
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    instance = singleton.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

/*
이 싱글턴을 써주려면  MonoBehaviour 자리에 Singleton<'스크립트 이름'> 이라고 적어주기
 */