using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤー側の通常剣攻撃。攻撃中はOverlapBaseの範囲判定でEnemyBaseに継続ダメージを与える。
/// </summary>
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
    /// <summary>
    /// 攻撃が有効な間、範囲内のEnemyBaseを検出して生存している敵に1回だけダメージを与える。
    /// </summary>
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
