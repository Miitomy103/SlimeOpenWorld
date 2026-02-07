using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

public class HumanHost : HostBase
{
    public StateMachine<HumanHost> stateMachine;

    public Animator animator { get; private set; }

    [SerializeField] HumanMove humanMove;
    public HumanMove HumanMove => humanMove;
    [SerializeField] HumanIdle humanIdle;
    public HumanIdle HumanIdle => humanIdle;
    [SerializeField] HumanDash humanDash;
    public HumanDash HumanDash => humanDash;
    [SerializeField] HumanAttack humanAttack;
    public HumanAttack HumanAttack => humanAttack;
    [SerializeField] HumanDashAttack humanDashAttack;
    public HumanDashAttack HumanDashAttack => humanDashAttack;

    public Vector3 currentVelocity = Vector3.zero;
    Vector3 velocityRef = Vector3.zero;

    public bool IsMoveInput
    {
        get
        {
            PlayerInput input = InputData.Instance.InputAction();
            Vector3 moveInput = new Vector3(input.Horizontal, 0, input.Vertical);
            return moveInput.magnitude > 0.05f;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine = new StateMachine<HumanHost>(this);
        stateMachine.ChangeState(humanMove);
    }

    public override void EndHost()
    {
        base.EndHost();
        gameObject.SetActive(false);
    }
    public override void UpdateHost()
    {
        base.UpdateHost();
        stateMachine.Update();
    }

    public void Movement(Vector3 moveInput, float targetSpeed, float smoothTime, float rotationSpeed)
    {
        Camera mainCamera = Camera.main;

        // カメラ基準の入力方向
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDir = (camForward * moveInput.z + camRight * moveInput.x).normalized;
        float inputMag = Mathf.Clamp01(moveInput.magnitude);

        // 慣性を考慮した移動速度の変化
        Vector3 targetVelocity = targetDir * targetSpeed * inputMag;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, smoothTime);

        // 実際に移動
        controller.Move(currentVelocity * Time.deltaTime);

        // スムーズな回転
        if (targetDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
