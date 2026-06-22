using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 骸骨(Skull)に取り憑いて突進攻撃を行うホスト。targetに接触すると攻撃して乗っ取りを解除する
/// </summary>
public class SkullHost : HostBase
{
    public Transform target { get; set; }
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] OverlapBase attackRange;

    [SerializeField] UnityEvent onAttackStart;

    bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        if(attackRange == null)
        {
            attackRange = GetComponentInChildren<OverlapBase>();
        }
    }
    public override void UpdateHost()
    {
        if (isAttacking) return;
        Vector3 dir = target.position - transform.position;
        if (dir.sqrMagnitude > 0.001f)
        {
            // 位置を移動
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

            // 回転（移動方向へ滑らかに向く）
            Quaternion targetRot = Quaternion.LookRotation(dir);
            targetRot = targetRot * Quaternion.Euler(0f, 180f, 0f); // 少し斜めに移動させる

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
        else
        {
            isAttacking = true;
            ForceHostBack();
        }

    }

    public override void EndHost()
    {
        base.EndHost();
        var boss=attackRange.OverlapAll<SkullBoss>("Enemy");
        if(boss.Length > 0)
        {
            boss[0].SkullDamage();
            onAttackStart?.Invoke();
        }
        gameObject.SetActive(false);
    }

}
