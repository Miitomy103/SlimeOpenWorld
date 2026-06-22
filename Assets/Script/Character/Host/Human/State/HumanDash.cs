using System.Collections;
using UnityEngine;
using InGame;

/// <summary>
/// 人間のダッシュステート。入力に応じてダッシュ攻撃/ジャンプ/通常移動へ遷移する
/// </summary>
[System.Serializable]
public class HumanDash : IState<HumanHost>
{
    [SerializeField] float dashSpeed = 18f;
    [SerializeField] float smoothTime = 0.1f; // 加減速の滑らかさ
    [SerializeField] float rotationSpeed = 10f; // 回転のスムーズさ
    public void DoExit(HumanHost owner)
    {

    }

    public void DoStart(HumanHost owner)
    {
        owner.animator.SetTrigger("Dash");
    }

    public void DoUpdate(HumanHost owner)
    {
        HandleInput(owner);
    }
    public void HandleInput(HumanHost owner)
    {
        PlayerInput input = InputData.Instance.InputAction();

        if(input.Action0.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanDashAttack);
            return;
        }
        if (input.Jump.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanJump);
            return;
        }

        Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);

        // 入力の強さで歩き・走りを切り替え
        float inputMag = Mathf.Clamp01(moveInput.magnitude);

        owner.Movement(moveInput, dashSpeed, smoothTime, rotationSpeed);


        if(moveInput.magnitude <= 0.05f)
        {
            owner.animator.SetBool("IsMove", false);
            owner.stateMachine.ChangeState(owner.HumanIdle);
        }
        else if (moveInput.magnitude < 0.6f)
        {
            owner.stateMachine.ChangeState(owner.HumanMove);
        }
    }

    private IEnumerator StopCoroutine(HumanHost owner)
    {
        yield return null;
    }
}
