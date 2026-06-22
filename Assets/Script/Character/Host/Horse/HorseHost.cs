using UnityEngine;
using InGame;

/// <summary>
/// 馬のホストクラス。乗っ取りが終わるとPossessHorseを残して非アクティブ化する
/// </summary>
public class HorseHost : HostBase
{
    public StateMachine<HorseHost> stateMachine;

    public Animator animator { get; private set; }
    [SerializeField] HorseMove horseMove;
    public HorseMove HorseMove => horseMove;
    Vector3 currentVelocity = Vector3.zero;
    Vector3 velocityRef = Vector3.zero;

    [SerializeField] PossessHorse possessHorse;

    public Move move { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }
    protected override void Start()
    {
        base.Start();
        move=new Move(transform, controller);
    }
    public override void StartHost(HostBase ago)
    {
        base.StartHost(ago);

        stateMachine = new StateMachine<HorseHost>(this);

        stateMachine.ChangeState(horseMove);
    }

    public override void UpdateHost()
    {
        base.UpdateHost();
        stateMachine.Update();
    }

    public override void EndHost()
    {
        base.EndHost();
        Instantiate(possessHorse, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
