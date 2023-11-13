using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// using UnityEngine.Pool 공부해서 그걸로 쓰기!
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    protected GameObject objectToPool;
    [SerializeField]
    protected int poolSize = 10;

    protected Queue<GameObject> objectPool;

    public Transform spawnedObjectParent;

    public bool alwaysDestroy = false;

    private void Awake()
    {
        objectPool = new Queue<GameObject>();
    }

    public void Initialized(GameObject objectToPool, int poolSize = 10)
    {
        this.objectToPool = objectToPool;
        this.poolSize = poolSize;
    }
    
    public GameObject CreateObject()
    {
        CreateObjectParentIfNeeded();

        GameObject spawnedObject = null;
        if(objectPool.Count < poolSize)
        {
            spawnedObject = Instantiate(objectToPool, transform.position, Quaternion.identity);
            spawnedObject.name = transform.root.name + "_" + objectToPool.name + "_" + objectPool.Count;
            spawnedObject.transform.SetParent(spawnedObjectParent);
            spawnedObject.AddComponent<DestroyIfDisabled>();
        }
        else
        {
            spawnedObject = objectPool.Dequeue();
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = Quaternion.identity;
            spawnedObject.SetActive(true);
        }

        objectPool.Enqueue(spawnedObject);
        return spawnedObject;
    }

    private void CreateObjectParentIfNeeded()
    {
        if(spawnedObjectParent == null)
        {
            string name="ObjectPool_"+objectToPool.name;
            var parentObject = GameObject.Find(name);
            if(parentObject != null)
            {
                spawnedObjectParent = parentObject.transform;
            }
            else
            {
                spawnedObjectParent = new GameObject(name).transform;
            }
        }
    }
    private void OnDestroy()
    {
        foreach(var item in objectPool)
        {
            if (item == null)
                continue;
            else if (item.activeSelf == false || alwaysDestroy) //트랙마크 다 지워주게 
                Destroy(item);
            else
                item.GetComponent<DestroyIfDisabled>().SelfDestructionEnabled = true;
        }   
    }
}
