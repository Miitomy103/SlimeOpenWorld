using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;
using UnityEngine.InputSystem;

/// <summary>
/// スライムのホストクラス
/// </summary>
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

    FollowUI followUI;

    [SerializeField] CameraPivotRotator cameraPivotRotator;
    public IRotatePivot RotatePivot => cameraPivotRotator;
    public bool IsAiming { get; set; } = true;
    [SerializeField]float defaultAimSpeed = 2f;

    [SerializeField,ReadOnly] string currentState;

    [SerializeField] DetectRange detectRange;
    [SerializeField] ProjectileLauncher projectileLauncherPrefab;
    ProjectileLauncher projectileLauncher;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        movement = new Movement(transform, controller,Camera.main.transform);
        rotator = new Rotator(transform);
    }

    protected override void Start()
    {
        base.Start();
        projectileLauncher = Instantiate(projectileLauncherPrefab, transform.position, Quaternion.identity);

        detectRange.Initialize(transform,transform);
        followUI = FollowUI.Instance;
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

    IPossess possessTarget;
    /// <summary>
    /// のっとり可能な対象を探して、のっとりを実行する
    /// </summary>
    void Possess()
    {
        IPossess[] possessTargets = detectRange.DetectComponents<IPossess>();
        if (possessTargets == null) return;
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

        if(followUI != null)
            followUI.UIUpdate(t);

        if (possessTarget == null) return;

        PlayerInput input = InputData.Instance.InputAction();

        if (input.Button3.onDown)
        {
            //乗っ取り演出
            this.possessTarget = possessTarget;
            projectileLauncher.gameObject.SetActive(true);
            projectileLauncher.transform.position = transform.position;
            projectileLauncher.Jump(possessTarget.Transform, () => Possession());
            Time.timeScale = 0f;
            Debug.Log("Time0");
            CameraManager.Instance.SetFollow(projectileLauncher.transform);
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// のっとりを実行する
    /// </summary>
    private void Possession()
    {
        Time.timeScale = 1f;
        projectileLauncher.gameObject.SetActive(false);
        PlayerController.Instance.SetHost(possessTarget.GetHost(), this);
        GameEvents.PossessionChanged(possessTarget.PossessId);

        followUI.UIUpdate(null);

        gameObject.SetActive(false);
    }
    public override void EndHost()
    {
        base.EndHost();
        if(stateMachine!=null) stateMachine.CurrentState.DoExit(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectRange != null)
        {
            detectRange.OnDrawGizmos();
        }
    }

    private void OnDestroy()
    {
        detectRange=null;
    }
}

