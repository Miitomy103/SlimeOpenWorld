using UnityEngine;
using InGame;

[System.Serializable]
public class HumanJump : IState<HumanHost>
{
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float gravity = -20f;
    [SerializeField] float airMoveSpeed = 4f;
    [SerializeField] float smoothTime = 0.15f;
    [SerializeField] float rotationSpeed = 8f;

    float verticalVelocity = 0f;
    bool hasLanded = false;

    public void DoStart(HumanHost owner)
    {
        verticalVelocity = jumpForce;
        hasLanded = false;

        owner.animator.SetTrigger("Jump");
        owner.animator.SetBool("IsGround", false);
    }

    public void DoUpdate(HumanHost owner)
    {
        // 重力を加算
        verticalVelocity += gravity * Time.deltaTime;

        // 空中での水平移動
        PlayerInput input = InputData.Instance.InputAction();
        Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);
        owner.Movement(moveInput, airMoveSpeed, smoothTime, rotationSpeed);

        // 垂直方向の移動を直接 controller に適用
        if (owner.controller.enabled && owner.controller.gameObject.activeInHierarchy)
        {
            owner.controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }

        // 着地判定
        bool isGrounded = owner.controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = 0f;

            if (!hasLanded)
            {
                hasLanded = true;
                owner.animator.SetBool("IsGround", true);
                Land(owner);
            }
        }
    }

    public void DoExit(HumanHost owner)
    {
        verticalVelocity = 0f;
        hasLanded = false;
        owner.animator.SetBool("IsGround", true);
    }

    void Land(HumanHost owner)
    {
        if (owner.IsMoveInput)
            owner.stateMachine.ChangeState(owner.HumanMove);
        else
            owner.stateMachine.ChangeState(owner.HumanIdle);
    }
}
