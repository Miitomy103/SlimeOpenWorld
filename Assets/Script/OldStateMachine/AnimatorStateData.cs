using UnityEngine;

/// <summary>
/// StateTypeとkeyを目印に、Animator上の対応するAnimatorStatemachineを探して保持するクラス。
/// </summary>
public class AnimatorStateData
{
    private readonly StateType stateType;
    public StateType StateType => stateType;

    public AnimatorStatemachine state { get;private set; }
    int key;

    public AnimatorStateData(StateType stateType, int key)
    {
        this.stateType = stateType;
        this.key = key;
    }
    /// <summary>
    /// Animatorにアタッチされた全AnimatorStatemachineの中から、stateTypeとkeyが一致するものを探す。
    /// </summary>
    public bool GetComponent(Animator animator)
    {
        AnimatorStatemachine[] animatorStatemachines = animator.GetBehaviours<AnimatorStatemachine>();
        foreach (var animState in animatorStatemachines)
        {
            if (animState.StateType == stateType&&animState.Key==key)
            {
                state = animState;
                return true;
            }
        }
        return false;
    }
}
