using UnityEngine;

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
