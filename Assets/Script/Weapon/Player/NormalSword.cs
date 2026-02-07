using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalSord : WeaponBase
{
    [SerializeField] OverlapBase overLapBase;

    public bool isAttack = false;

    List<EnemyBase> hitEnemyList = new List<EnemyBase>();

    [SerializeField] UnityEvent onAttackStart;
    [SerializeField] UnityEvent onAttackEnd;


    private void OnEnable()
    {
        if (overLapBase == null) overLapBase = GetComponent<OverlapBase>();
    }
    private void OnDisable()
    {
        isAttack = false;
        hitEnemyList.Clear();
        overLapBase = null;
    }
    private void FixedUpdate()
    {
        if (isAttack)
        {
            EnemyBase[] enemies = overLapBase.OverlapAll<EnemyBase>();
            Debug.Log(enemies.Length);
            foreach (var host in enemies)
            {
                if (host != null&&!host.isDead)
                {
                    if (hitEnemyList.Contains(host))
                    {
                        continue;
                    }
                    hitEnemyList.Add(host);
                    host.TakeDamage(AttackPower);
                }
            }
        }
    }
    public override void AttackStart(IAttackCoroutine attackCoroutine)
    {
        attackCoroutine.AttackCoroutineAsync(() =>
        {
            isAttack = true;
            onAttackStart?.Invoke();
        },
        () =>
        {
            isAttack = false;
            hitEnemyList.Clear();
            onAttackEnd?.Invoke();
        });
    }
}
