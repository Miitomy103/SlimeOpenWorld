using Cysharp.Threading.Tasks;
using UnityEngine;
using InGame;

[System.Serializable]
public class HumanAttack : IState<HumanHost>,ICanBool
{
    [SerializeField] WeaponBase weaponBase;
    AnimatorStateData humanAttack;

    int attackCount = 0;

    bool isAttack;
    bool waitAttack;

    public bool IsComboInput = false;

    IAttackCoroutine[] attackCoroutines;

    bool endAsync;
    public bool CanBool => !endAsync;

    public void DoExit(HumanHost owner)
    {
        if (humanAttack?.state != null)
        {
            humanAttack.state.OnAttackEnd = null;
        }
        humanAttack = null;

        owner.animator.SetBool("IsAttack", false);
        attackCount = 0;
        isAttack = false;
        waitAttack = false;

        EndCoroutine().Forget();
    }

    async UniTask EndCoroutine()
    {
        endAsync = true;
        await UniTask.WaitForSeconds(0.1f);
        endAsync = false;
    }

    public void DoStart(HumanHost owner)
    {
        attackCoroutines = new IAttackCoroutine[]
        {
            new AnimAttackAcyncFrame(14,27-14,30,1.5f,null,()=>IsComboInput=true),
        };

        attackCount = 0;
        //owner.animator.SetBool("IsAttack", true);
        owner.animator.SetTrigger("Attack");
        AttackStart(owner).Forget();
        isAttack = false;
        waitAttack = false;

    }

    public void DoUpdate(HumanHost owner)
    {
        if (isAttack&&IsComboInput)
        {
            PlayerInput input = InputData.Instance.InputAction();
            Debug.Log("Waiting for Attack Input");

            if (input.Action0.onDown) 
            {
                Debug.Log("Attack Input Received");
                waitAttack = true;
                //owner.animator.SetBool("IsAttack", true);
                owner.animator.SetTrigger("Attack");
                IsComboInput = false;
            }
        }
    }

    async UniTask AttackStart(HumanHost owner)
    {
        await UniTask.NextFrame();

        if(humanAttack != null && humanAttack.state != null)
        {
            humanAttack.state.OnAttackEnd = null;
        }
        humanAttack = new AnimatorStateData(StateType.Attack, attackCount);
        if (humanAttack.GetComponent(owner.animator))
        {
            humanAttack.state.OnAttackEnd += () => AttackEnd(owner);
        }
        weaponBase.AttackStart(attackCoroutines[attackCount]);

        await UniTask.NextFrame();
        waitAttack = false;
        //owner.animator.SetBool("IsAttack",false);
        if(attackCount < attackCoroutines.Length) isAttack = true;
        attackCount++;
    }

    void AttackEnd(HumanHost owner)
    {
        isAttack = false;

        bool canCombo = attackCount < attackCoroutines.Length && waitAttack;

        //owner.animator.SetBool("IsAttack", canCombo);

        // (Debug.Logは waitAttack を表示しているので、そのままでOKです)
        Debug.Log($"Can Combo: {canCombo}, Attack Count: {attackCount},maxAttackCount:{attackCoroutines.Length},WaitAttack:{waitAttack}");

        if (canCombo)
        {
            AttackStart(owner).Forget();
            return;
        }

        // ここまで来たら攻撃終了確定
        waitAttack = false;
        //owner.animator.SetBool("IsAttack", false);

            if (owner.IsMoveInput)
                owner.stateMachine.ChangeState(owner.HumanMove);
            else
                owner.stateMachine.ChangeState(owner.HumanIdle);
    }
}
