using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

/// <summary>
/// SkullBossの徘徊ステート。正面方向に一定時間歩いたらIdleへ戻る
/// </summary>
public class SkullBossWalk : StateBase<string>, ITransitionCondition<string>
{
    public const string key = "SkullBossWalk";
    public override string Key => key;

    [SerializeField] float speed = 3f;

    CharacterController controller;
    Animator animator;
    IIsAutoRotator isAutoRotator;

    [SerializeField]float walkTime = 3f;
    bool isWalk;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if(controller == null) controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (isAutoRotator == null) isAutoRotator = GetComponent<IIsAutoRotator>();
        animator.SetBool("IsMove", true);
        isWalk = true;
        StartCoroutine(Coroutine());
    }

    public override void OnStateUpdate()
    {
        Vector3 move = transform.forward * speed;
        controller.Move(move * Time.deltaTime);
        base.OnStateUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        animator.SetBool("IsMove", false);
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(walkTime);
        isWalk = false;
    }
    protected override List<ITransitionCondition<string>> CreateCondition()
    {
        return new List<ITransitionCondition<string>>()
        {
            this
        };
    }

    public bool CanTransition()
    {
        return !isWalk;
    }

    public void Transition()
    {
        transition.Transition(SkullBossIdle.key);
    }
}
