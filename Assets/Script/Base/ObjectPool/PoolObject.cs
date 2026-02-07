using System;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject, IDisposable
{
    public event Func<IPoolObject, bool> DoReturnToPool;

    public virtual void DoDisable() => gameObject.SetActive(false);

    public void DoEnable() => gameObject.SetActive(true);

    protected virtual void OnDestroy() => Dispose();

    public void Dispose()
    {
        DoReturnToPool = null;
    }
}