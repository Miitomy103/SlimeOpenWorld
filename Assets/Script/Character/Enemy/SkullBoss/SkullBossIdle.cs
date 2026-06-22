using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

/// <summary>
/// SkullBossの待機ステート。idleTime経過後、ランダムでWalk/Attackへ遷移する
/// </summary>
public class SkullBossIdle : StateBase<string>, ITransitionCondition<string>
{
    public const string key = "SkullBossIdle";
    public override string Key => key;

    [SerializeField]float idleTime = 2f;

    protected override List<ITransitionCondition<string>> CreateCondition()
    {
        return new List<ITransitionCondition<string>>()
        {
            this
        };
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        isAttack = false;
        StartCoroutine(EnterCoroutine());
    }

    bool isAttack;
    IEnumerator EnterCoroutine()
    {
        yield return new WaitForSeconds(idleTime);
        isAttack = true;

    }

    public bool CanTransition()
    {
        return isAttack;
    }

    public void Transition()
    {
        int r=Random.Range(0, 2);
        if(r==0) transition.Transition(SkullBossWalk.key);
        else transition.Transition(SkullBossAttack.key);
    }
}
