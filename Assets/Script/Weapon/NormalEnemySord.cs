using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemySord : WeaponBase
{
    [SerializeField] OverlapBase overLapBase;

    public bool isAttack = false;

    List<HostBase> hitHostList = new List<HostBase>();


    private void OnEnable()
    {

        if(overLapBase==null) overLapBase = GetComponent<OverlapBase>();
    }
    private void OnDisable()
    {
        isAttack = false;
        hitHostList.Clear();
        overLapBase = null;
    }
    private void FixedUpdate()
    {
        if(isAttack)
        {
            HostBase[] hostBases = overLapBase.OverlapAll<HostBase>();
            foreach (var host in hostBases)
            {
                if (host != null && host == PlayerController.Instance.HostBase)
                {
                    if (hitHostList.Contains(host))
                    {
                        continue;
                    }
                    hitHostList.Add(host);
                    host.Damage(AttackPower);
                }
            }
        }
    }
    public override void AttackStart(IAttackCoroutine attackCoroutine)
    {
        attackCoroutine.AttackCoroutineAsync(() =>
        {
            isAttack = true;
        },
        () =>
        {
            isAttack = false;
            hitHostList.Clear();
        });
    }
}
