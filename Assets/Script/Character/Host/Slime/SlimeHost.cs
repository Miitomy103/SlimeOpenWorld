using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;
using UnityEngine.InputSystem;

public class SlimeHost : HostBase
{
    public StateMachine<SlimeHost> stateMachine;

    public Animator animator { get; private set; }

    [Header("ValueSettings")]
    [SerializeField] Vector3 startHostOffset;
    public Movement movement{ get;private set; }
    public Rotator rotator { get; private set; }
    #region States
    [Header("States")]
    [SerializeField] SlimeIdle slimeIdle;
    public SlimeIdle SlimeIdle => slimeIdle;

    [SerializeField] SlimeMove slimeMove;
    public SlimeMove SlimeMove => slimeMove;
    [SerializeField] SlimeAim slimeAim;
    public SlimeAim SlimeAim => slimeAim;

    [SerializeField] SlimeAttack slimeAttack;
    public SlimeAttack SlimeAttack => slimeAttack;
    #endregion

    PossessRange possessRange;

    [SerializeField] FollowUI followUI;

    [SerializeField] PlayerController playerController;

    [SerializeField] CameraPivotRotator cameraPivotRotator;
    public IRotatePivot RotatePivot => cameraPivotRotator;
    public bool IsAiming { get; set; } = true;
    [SerializeField]float defaultAimSpeed = 2f;

    [SerializeField,ReadOnly] string currentState;

    [SerializeField] DetectRange detectRange;



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        possessRange = GetComponentInChildren<PossessRange>();
        movement = new Movement(transform, controller);
        rotator = new Rotator(transform);
    }

    public override void StartHost(HostBase ago)
    {

        if(ago!=null) transform.position = ago.transform.position;

        if (ago != null) transform.position = ago.transform.position + startHostOffset;
        gameObject.SetActive(true);

        base.StartHost(ago);
        stateMachine = new StateMachine<SlimeHost>(this);
        stateMachine.OnStateChanged += (from) =>
        {
            currentState = from.GetType().Name;
        };
        stateMachine.ChangeState(slimeIdle);
    }

    public override void UpdateHost()
    {
        base.UpdateHost();
        if(stateMachine!=null) stateMachine.Update();

        Possess();

        if (IsAiming&&RotatePivot!=null) RotatePivot.RotatePivot(Input.Axis, defaultAimSpeed);
    }

    void Possess()
    {
        IPossess[] possessTargets = detectRange.DetectComponents<IPossess>();
        IPossess possessTarget=null;

        for (int i = 0; i < possessTargets.Length; i++)
        {
            if (possessTargets[i].CanPossess&&
                (possessTarget==null||
                Vector3.Distance(transform.position, possessTargets[i].Transform.position) <Vector3.Distance(transform.position, possessTarget.Transform.position)))
            {
                possessTarget = possessTargets[i];
            }
        }
                
        Transform t = possessTarget != null ? possessTarget.Transform : null;

        followUI?.UIUpdate(t);

        if (possessTarget == null) return;

        PlayerInput input = InputData.Instance.InputAction();

        if (input.Button3.onDown)
        {
            playerController.SetHost(possessTarget.GetHost(),this);
            GameEvents.PossessionChanged(possessTarget.PossessId);

            followUI?.UIUpdate(null);

            gameObject.SetActive(false);
        }
    }
    public override void EndHost()
    {
        base.EndHost();
        stateMachine.CurrentState.DoExit(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectRange != null)
        {
            detectRange.OnDrawGizmos();
        }
    }
}

