using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // ���� �������� �ؾ��� ��
    public void OnBeforeSerialize()
    {
        keys.Clear(); values.Clear();

        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // ���Ϸκ��� �ҷ��� ������ �ؾ��� ��
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
        {
            Debug.LogError("key count does no match to value count");
        }
        else
        {
            for (int i = 0; i < keys.Count; ++i)
            {
                this.Add(keys[i], values[i]);
            }
        }
    }
}