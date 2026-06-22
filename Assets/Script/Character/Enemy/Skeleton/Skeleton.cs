using System;
using UnityEngine;
using UnityEngine.AI;
using InGame;

/// <summary>
/// スケルトンの敵クラス。ステートマシンでの行動制御とNPCへの乗っ取り(Possess)対応を行う
/// </summary>
public class Skeleton : EnemyBase,IPossess
{
    public StateMachine<Skeleton> stateMachine { get; private set; }
    public Animator animator { get; private set; }
    public NavMeshAgent agent { get; private set; }

    [SerializeField] SkeletonIdle skeletonIdle;
    public SkeletonIdle SkeletonIdle => skeletonIdle;
    [SerializeField] SkeletonButtle skeletonButtle;
    public SkeletonButtle SkeletonButtle => skeletonButtle;
    [SerializeField] SkeletonBack skeletonBack;
    public SkeletonBack SkeletonBack => skeletonBack;
    [SerializeField] SkeletonDie skeletonDie;
    public SkeletonDie SkeletonDie => skeletonDie;

    public Action OnDie { get; set; }

    [SerializeField]Move move;
    public Move Move => move;


    [Header("Possess")]
    [SerializeField] HumanHost hostPrefab;
    [SerializeField] string possessId;
    public string PossessId => possessId;

    public bool CanPossess => isDead;

    IPoolObject hpvar;
    ISlider hpSlider;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Skeleton>(this);
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }
    override protected void Start()
    {
        base.Start();
        stateMachine.ChangeState(skeletonIdle);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (hpSlider != null)
        {
            hpSlider.SetValue((float)CurrentHealth , maxHealth);
        }
    }
    public override void OnDetector(bool isDetect)
    {
        if (isDetect)
        {
            stateMachine.ChangeState(skeletonButtle);
            if (hpvar == null)
            {
                hpvar = EnemyHpVarPool.Instance.AddTarget(transform);
                hpSlider = hpvar as ISlider;
            }
        }
        else
        {
            stateMachine.ChangeState(skeletonIdle);
            hpvar.DoDisable();
            hpSlider = null;
            hpvar = null;
        }
    }
    public override void DoUpdate()
    {
        base.DoUpdate();
        stateMachine.Update();
    }
    public void MoveSpeedAnimation()
    {
        animator.SetBool("IsMove", true);
        Transform host = PlayerController.Instance.HostBase.transform;
        Vector3 toPlayer = host.position - transform.position;
        float speedAlongPlayerDir = Vector3.Dot(agent.velocity, toPlayer.normalized);
        animator.SetFloat("Speed", speedAlongPlayerDir);

    }
    protected override void Die()
    {
        stateMachine.ChangeState(SkeletonDie);
        hpvar.DoDisable();
        Debug.Log("Skeleton Die");
        base.Die();
    }

    public HostBase GetHost()
    {
        gameObject.SetActive(false);
        return Instantiate(hostPrefab, transform.position, transform.rotation);
    }
}
