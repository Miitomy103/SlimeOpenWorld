using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPool<T> where T : Object, IPoolObject
{
    [SerializeField]
    T objectPrefab;

    [SerializeField]
    int capacity = 100;

    [SerializeField]
    bool canExpansion = true;

    [SerializeField]
    bool isCollectionCheck = false;

    Stack<T> poolStack;
    List<T> enableObjects;
    bool isReturnToStack = true;
    Transform parent;

    public int EnableCount => enableObjects.Count;
    public IReadOnlyList<T> EnableObjects => enableObjects;

    public ObjectPool(T objectPrefab)
    {
        this.objectPrefab = objectPrefab;

        capacity = 0;
        canExpansion = true;
        poolStack = new Stack<T>();
        enableObjects = new List<T>();
    }

    public ObjectPool(T objectPrefab, int capacity,Transform parent=null, bool canExpansion = true, bool isCollectionCheck = false)
    {
        this.objectPrefab = objectPrefab;
        this.capacity = capacity;
        this.canExpansion = canExpansion;
        this.isCollectionCheck = isCollectionCheck;
        this.parent = parent;

        poolStack = new Stack<T>(capacity);
        enableObjects = new List<T>(capacity);

        for (int index = 0; index < capacity; index++)
        {
            var obj = Object.Instantiate(objectPrefab,parent?parent:null);
            obj.DoDisable();
            obj.DoReturnToPool += ReturnToPoolObject;
            poolStack.Push(obj);
        }

        this.isCollectionCheck = isCollectionCheck;
    }

    public T EnableToPoolObject()
    {
        if (poolStack.Count == 0)
        {
            if (!canExpansion)
            {
                Debug.LogWarning("There are no objects stacked in the pool");
                return null;
            }

            var addObj = Object.Instantiate(objectPrefab,parent?parent:null);
            addObj.DoReturnToPool += ReturnToPoolObject;
            enableObjects.Add(addObj);
            capacity++;
            return addObj;
        }

        var obj = poolStack.Pop();
        enableObjects.Add(obj);
        obj.DoEnable();
        return obj;
    }

    bool ReturnToPoolObject(IPoolObject obj) => ReturnToPoolObject(obj as T);

    public bool ReturnToPoolObject(T obj)
    {
        if (!isReturnToStack)
        {
            return true;
        }

        if (obj == null)
        {
            Debug.LogWarning("The target object is null");
            return false;
        }

        if (isCollectionCheck)
        {
            if (!enableObjects.Contains(obj))
            {
                Debug.LogWarning("The target object is not in the active pool");
                return false;
            }
        }

        enableObjects.Remove(obj);
        poolStack.Push(obj);
        return true;
    }

    public void ReturnToAllEnableObjects()
    {
        isReturnToStack = false;
        foreach (var obj in enableObjects)
        {
            obj.DoDisable();
            poolStack.Push(obj);
        }
        enableObjects.Clear();
        isReturnToStack = true;
    }
}