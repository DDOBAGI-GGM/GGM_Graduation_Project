using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour       // �׳� �Ǳ���
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
�� �̱����� ���ַ���  MonoBehaviour �ڸ��� Singleton<'��ũ��Ʈ �̸�'> �̶�� �����ֱ�
 */