using UnityEngine;
using StateMachine;
using System.Collections.Generic;
using System.Collections;
public class SkullBossAttack : StateBase<string>,ITransitionCondition<string>
{
    public static string key = "SkullBossAttack";
    public override string Key => key;

    [SerializeField] SkullPossess skullPossess;
    [SerializeField] Transform thisTarget;
    [SerializeField] Animator animator;

    [SerializeField] AnimationEventAction animationEventAction;

    AnimatorStateData animatorStateData;

    bool isAttack;

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if(animationEventAction == null)animationEventAction = GetComponentInChildren<AnimationEventAction>();
    }
    private void Start()
    {
        animatorStateData = new AnimatorStateData(StateType.Attack, 0);
        if (animatorStateData.GetComponent(animator))
        {
            animatorStateData.state.OnAttackEnd += AttackEnd;
        }

        animationEventAction.AddEventAction("attack1Start", IsAttack);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        isAttack = true ;

        animator.SetTrigger("Attack");
        animator.SetInteger("AttackValue", 1);
    }
    void AttackEnd()
    {
        isAttack = false;
    }
     void IsAttack()
    {
        var skull = Instantiate(skullPossess, thisTarget.position, Quaternion.identity);
        skull.Initialize(thisTarget);
    }


    public bool CanTransition()
    {
        return !isAttack;
    }

    public void Transition()
    {
        transition.Transition(SkullBossIdle.key);
    }

    protected override List<ITransitionCondition<string>> CreateCondition()
    {
        return new List<ITransitionCondition<string>>()
        {
            this
        };
    }
}
