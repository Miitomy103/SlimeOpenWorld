using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵側の通常剣攻撃。攻撃中はOverlapBaseの範囲判定でHostBaseに継続ダメージを与える。
/// </summary>
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
    /// <summary>
    /// 攻撃が有効な間、範囲内のHostBaseを検出してプレイヤーであれば1回だけダメージを与える。
    /// </summary>
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
