using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : Component
{
    private List<T> pooledObjects = new();
    private T prefab;
    private Transform parentTransform;

    public ObjectPooler(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentTransform = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    public T GetObject()
    {
        foreach (T obj in pooledObjects)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        T newObj = CreateObject();
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void DestroyPool()
    {
        foreach (T obj in pooledObjects)
        {
            if (obj != null)
            {
                Object.Destroy(obj.gameObject);
            }
        }

        pooledObjects.Clear();
        pooledObjects = null;
    }

    private T CreateObject()
    {
        T newObj = Object.Instantiate(prefab, parentTransform);
        newObj.gameObject.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }
}
