using System;
using UnityEngine;

/// <summary>
/// Animatorのステート開始/終了に合わせてOnAttackStart/OnAttackEndを発火するStateMachineBehaviour。
/// stateType・keyはAnimatorStateDataから同じ組を検索する際の目印。
/// </summary>
public class AnimatorStatemachine : StateMachineBehaviour
{
    [SerializeField] StateType stateType;
    public StateType StateType => stateType;
    [SerializeField] int key;
    public int Key => key;
    public Action OnAttackStart;
    public Action OnAttackEnd;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        OnAttackStart?.Invoke();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        OnAttackEnd?.Invoke();
    }
}
