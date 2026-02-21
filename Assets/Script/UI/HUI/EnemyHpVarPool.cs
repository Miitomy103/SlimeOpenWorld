using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHpVarPool : MonoBehaviour
{
    [SerializeField] FollowHPVar varPrefab;
    [SerializeField] Transform parent;
    IObjectPool<FollowHPVar> objectPool;

    private static EnemyHpVarPool instance;
    public static EnemyHpVarPool Instance=>instance;

    CullingGroup cullingGroup;

    private void Awake()
    {
        if (instance == null)instance = this;
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        objectPool = new ObjectPool<FollowHPVar>(varPrefab, 0, parent);
    }

    public IPoolObject AddTarget(Transform target)
    {
        var hpVar = objectPool.EnableToPoolObject();
        hpVar.SetTarget(target);
        return hpVar;
    }
}
