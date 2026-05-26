using UnityEngine;
using UnityEngine.Events;

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
            // ˆÊ’u‚ğˆÚ“®
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

            // ‰ñ“]iˆÚ“®•ûŒü‚ÖŠŠ‚ç‚©‚ÉŒü‚­j
            Quaternion targetRot = Quaternion.LookRotation(dir);
            targetRot = targetRot * Quaternion.Euler(0f, 180f, 0f); // ­‚µÎ‚ß‚ÉˆÚ“®‚³‚¹‚é

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
