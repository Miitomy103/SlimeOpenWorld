using UnityEngine;
using Cysharp.Threading.Tasks;
using InGame;

/// <summary>
/// スケルトンの戦闘ステート。攻撃範囲まで接近し、武器のAttackStartで攻撃する
/// </summary>
[System.Serializable]
public class SkeletonButtle : IState<Skeleton>
{
    [SerializeField]float attackDistance = 2.0f;

    AnimatorStateData skeletonAttack;

    [SerializeField] WeaponBase weapon;

    [SerializeField]float attackMoveSpeed = 2.0f;



    bool isAttack = false;
    bool isBack;
    public void DoExit(Skeleton owner)
    {
        if (skeletonAttack?.state != null) skeletonAttack.state.OnAttackEnd = null;

        skeletonAttack = null;
    }


    public void DoStart(Skeleton owner)
    {
        isBack = false;
        skeletonAttack=new AnimatorStateData(StateType.Attack,0);
        if (skeletonAttack.GetComponent(owner.animator))
        {
           skeletonAttack.state.OnAttackEnd += () => AttackEnd(owner);
        }
    }

    public void DoUpdate(Skeleton owner)
    {
        Chase(owner);
        if(isAttack)AttackNow(owner);
    }
    void AttackNow(Skeleton owner)
    {
        owner.SkeletonBack.RotationSet(owner);
    }
    private void Chase(Skeleton owner)
    {
        if(isAttack||isBack)
        {
            return;
        }
        Transform host = PlayerController.Instance.HostBase.transform;
        float distance = Vector3.Distance(owner.transform.position, host.position);
        if (distance > attackDistance)
        {
            // プレイヤーに向かって移動
            owner.agent.isStopped = false;
            owner.agent.SetDestination(host.position);

            owner.MoveSpeedAnimation();

            owner.animator.SetBool("IsMove", true);

        }
        else
        {
            owner.animator.SetBool("IsMove", false);
            // 攻撃範囲内なら停止して攻撃
            owner.agent.isStopped = true;
            Attack(owner);
        }
    }
    private void Attack(Skeleton owner)
    {
        owner.animator.SetTrigger("Attack");
        isAttack = true;

        weapon.AttackStart(
            new DefaultAttackCoroutine(0.3f, 0.5f)
            );
    }
    private void AttackEnd(Skeleton owner)
    {
        Debug.Log("AttackEnd");
        isAttack = false;
        isBack = true;
        owner.stateMachine.ChangeState(owner.SkeletonBack);
    }
}
