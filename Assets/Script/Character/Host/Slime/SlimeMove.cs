using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

[System.Serializable]
public class SlimeMove : IState<SlimeHost>
{
    [SerializeField] float speed = 3f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField]Vector3 velocity= Vector3.zero;

    [SerializeField] Camera rotationCamera;  
    public void HandleInput(SlimeHost owner)
    {
        PlayerInput input = InputData.Instance.InputAction();

        if (input.Move == Vector2.zero)
        {
            owner.animator.SetBool("IsWalk", false);
            return;
        }

        Vector3 dire= owner.movement.HandleInput(speed, input.Move, velocity);
        owner.rotator.RotateTowards(dire, rotateSpeed);

        owner.animator.SetBool("IsWalk", true);
    }

    public void DoStart(SlimeHost owner)
    {

    }

    public void DoUpdate(SlimeHost owner)
    {
        HandleInput(owner);
        CheckCondition(owner);
    }

    void CheckCondition(SlimeHost owner)
    {
        if(owner.Input.Move.magnitude < 0.1f)
        {
            owner.stateMachine.ChangeState(owner.SlimeIdle);
            return;
        }
        if (owner.Input.Action1.onDown)
        {
            owner.stateMachine.ChangeState(owner.SlimeAim);
        }
        if(owner.Input.Action0.onDown)
        {
            owner.stateMachine.ChangeState(owner.SlimeAttack);
        }
    }

    public void DoExit(SlimeHost owner)
    {
        owner.animator.SetBool("IsWalk", false);
    }
}

