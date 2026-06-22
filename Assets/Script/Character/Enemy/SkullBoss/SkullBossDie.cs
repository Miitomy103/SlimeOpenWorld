using System.Collections.Generic;
using UnityEngine;
using StateMachine;

/// <summary>
/// SkullBossの死亡ステート。死亡アニメーションを再生し、以降のステート遷移は行わない
/// </summary>
public class SkullBossDie : StateBase<string>
{
    public const string key = "SkullBossDie";
    public override string Key => key;

    protected override List<ITransitionCondition<string>> CreateCondition()
    {
        return new List<ITransitionCondition<string>>()
        {
            // No transitions from Die state
        };
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.CrossFadeInFixedTime("Death", 0.1f);
        }
    }
}
