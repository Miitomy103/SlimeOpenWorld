using UnityEngine;
using InGame;

/// <summary>
/// 人間のダッシュ攻撃ステート。慣性を減衰させながら移動し、攻撃終了でIdleへ戻る
/// </summary>
[System.Serializable]
public class HumanDashAttack : IState<HumanHost>
{
    [SerializeField] WeaponBase weaponBase;
    AnimatorStateData humanAttack;

    public float drag = 2f;         // 慣性減衰の速さ
    public void DoExit(HumanHost owner)
    {
        if (humanAttack?.state != null)
        {
            humanAttack.state.OnAttackEnd = null;
        }
        humanAttack = null;
    }


    public void DoStart(HumanHost owner)
    {
        owner.animator.SetTrigger("DashAttack");
        humanAttack = new AnimatorStateData(StateType.Attack, 10);
        if (humanAttack.GetComponent(owner.animator))
        {
            humanAttack.state.OnAttackEnd += () => AttackEnd(owner);
        }
        weaponBase.AttackStart(
                new AnimAttackAcyncFrame(10, 28 - 10, 30, 1.5f)
            );
    }

    public void DoUpdate(HumanHost owner)
    {
        if(owner.currentVelocity.sqrMagnitude == 0)
        {
            return;
        }
        // 慣性による減速
        owner.currentVelocity = Vector3.Lerp(owner.currentVelocity, Vector3.zero, drag * Time.deltaTime);

        // 移動
        owner.controller.Move(owner.currentVelocity * Time.deltaTime);

        // 速度がほぼゼロになったら停止
        if (owner.currentVelocity.sqrMagnitude < 0.01f)
        {
            owner.currentVelocity = Vector3.zero;
            // 必要ならステート切り替え
        }
    }

    void AttackEnd(HumanHost owner)
    {
        owner.stateMachine.ChangeState(owner.HumanIdle);
    }
}
