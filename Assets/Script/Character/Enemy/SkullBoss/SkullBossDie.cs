using UnityEngine;
using StateMachine;
using System.Collections.Generic;

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
