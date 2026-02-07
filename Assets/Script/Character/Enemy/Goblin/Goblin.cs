using UnityEngine;
using InGame;

public class Goblin : EnemyBase
{
    public StateMachine<Goblin> stateMachine { get; private set; }

    public Animator animator { get; private set; }

    [SerializeField] GoblinIdle goblinIdle;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Goblin>(this);
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }
    override protected void Start()
    {
        base.Start();
        stateMachine.ChangeState(goblinIdle);
    }

    public override void OnDetector(bool isDetect)
    {
        throw new System.NotImplementedException();
    }
}
