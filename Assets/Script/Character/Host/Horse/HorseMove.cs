using UnityEngine;
using InGame;

[System.Serializable]
public class HorseMove : IState<HorseHost>
{
    [SerializeField]float speed = 5f;

    [SerializeField] float smoothTime = 0.1f; // 加減速の滑らかさ
    [SerializeField] float rotationSpeed = 10f; // 回転のスムーズさ
    public void DoExit(HorseHost owner)
    {

    }

    public void DoStart(HorseHost owner)
    {

    }
    public bool HandleInput(HorseHost owner)
    {
        PlayerInput input = InputData.Instance.InputAction();

        Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);

        if (input.Button0.onButton) moveInput *= 0.5f;


        float targetSpeed = moveInput.magnitude * speed;

        owner.move.Movement(moveInput, targetSpeed, smoothTime, rotationSpeed);

        // 🚀 ここを変更：「入力がある」時点で即走りモーション
        bool hasInput = moveInput.magnitude > 0.05f;
        owner.animator.SetBool("IsMove", hasInput);
        owner.animator.SetFloat("MoveSpeed", moveInput.magnitude);

        return hasInput;
    }

    public void DoUpdate(HorseHost owner)
    {
        bool hasInput = HandleInput(owner);

        //if (!hasInput)
        //{
        //    owner.stateMachine.ChangeState(owner.HumanIdle);
        //    return;
        //}

        //PlayerInput input = InputData.Instance.InputAction();
        //if (input.Action1.onDown)
        //{
        //    owner.stateMachine.ChangeState(owner.HumanDash);
        //}
    }
}
