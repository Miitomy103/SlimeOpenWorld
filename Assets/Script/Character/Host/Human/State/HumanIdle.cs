using UnityEngine;
using InGame;

[System.Serializable]
public class HumanIdle : IState<HumanHost>
{
    public void DoExit(HumanHost owner)
    {
    }

    public void DoStart(HumanHost owner)
    {
        owner.animator.SetBool("IsMove", false);
        owner.animator.SetFloat("MoveSpeed", 0);
    }

    public void DoUpdate(HumanHost owner)
    {
        PlayerInput input = InputData.Instance.InputAction();

        Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);

        if (moveInput.magnitude > 0.05f)
        {
            owner.stateMachine.ChangeState(owner.HumanMove);
            return;
        }
        if(input.Action0.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanAttack);
            return;
        }
        if (input.Jump.onDown)
        {
            owner.stateMachine.ChangeState(owner.HumanJump);
            return;
        }
    }
}
