using UnityEngine;
using InGame;

/// <summary>
/// 人間の移動ステート。入力の強さで歩き/走りを切り替える
/// </summary>
[System.Serializable]
public class HumanMove : IState<HumanHost>
{
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float smoothTime = 0.1f; // 加減速の滑らかさ
    [SerializeField] float rotationSpeed = 10f; // 回転のスムーズさ

    [SerializeField, ReadOnly] bool isWalk;

    public bool HandleInput(HumanHost owner)
    {
        PlayerInput input = InputData.Instance.InputAction();

        Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);

        if(input.Button0.onButton) moveInput *= 0.5f;

        MoveState moveState;

        if (moveInput.magnitude > 0.6f) moveState = MoveState.Run;
        else moveState = MoveState.Walk;

        // 入力の強さで歩き・走りを切り替え
        float inputMag = Mathf.Clamp01(moveInput.magnitude);

        float targetSpeed = moveState switch
        {
            MoveState.Walk => walkSpeed,
            MoveState.Run => runSpeed,
            _ => runSpeed,
        };
        owner.Movement(moveInput, targetSpeed, smoothTime, rotationSpeed);

        // 🚀 ここを変更：「入力がある」時点で即走りモーション
        bool hasInput = moveInput.magnitude > 0.05f;
        owner.animator.SetBool("IsMove", hasInput);
        owner.animator.SetFloat("MoveSpeed", (int)moveState);

        return hasInput;
    }

    public void DoStart(HumanHost owner)
    {
    }
    public void DoUpdate(HumanHost owner)
    {
        bool hasInput= HandleInput(owner);

        if(!hasInput)
        {
            owner.stateMachine.ChangeState(owner.HumanIdle);
            return;
        }

        PlayerInput input = InputData.Instance.InputAction();
        if (input.Action1.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanDash);
        }
        if(input.Action0.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanAttack);
        }
        if (input.Jump.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanJump);
            return;
        }
    }
    public void DoExit(HumanHost owner) { }
    public enum MoveState
    {
        Walk,
        Run,
        Dash
    }
}
