using Unity.VisualScripting;
using UnityEngine;

public class SkullPossess : PossessBase,IEnemy
{
    public override bool CanPossess => (Mathf.InverseLerp(0, maxHp, currentHp) * 100) <= parsent;
    private Transform bossTransform;

    [Header("HP")]
    [SerializeField] int maxHp = 100;
    [SerializeField] int currentHp;
    [SerializeField, Range(0, 100)] int parsent;

    [SerializeField] SkullHost skullHost;

    [Header("Move")]
    private Transform player => PlayerController.Instance.HostBase.transform;

    public int CurrentHealth => currentHp;

    public bool isDead => currentHp<=0;

    [SerializeField] private string enemyID = "DefaultEnemy";
    public string EnemyId =>enemyID;

    [SerializeField]float moveSpeed = 5f;
    [SerializeField]float moveTime = 5f;
    float time;
    //Œ¸‘¬
    private float deceleration = 0f;
    public override HostBase GetHost()
    {
        SkullHost skull= Instantiate(skullHost,transform.position,Quaternion.identity);
        skull.target = bossTransform;
        gameObject.SetActive(false);
        return skull;
    }

    public void Initialize(Transform boss)
    {
        bossTransform = boss;
    }
    private void OnEnable()
    {
        time = 0f;
        deceleration = 0f;
    }
    void Start()
    {
        currentHp = maxHp;
    }
    
    void Update()
    {
        Move();
        Rotation();

        if (deceleration > 0f)
        {
            deceleration -= Time.deltaTime/10;
        }
    }
    void Move()
    {
        if (player == null) return;
        transform.Translate(Vector3.forward * moveSpeed * (1 - deceleration) * Time.deltaTime);
    }

    void Rotation()
    {
        time += Time.deltaTime;
        if(time > moveTime) return;
        HostBase player = PlayerController.Instance.HostBase;
        if (player == null||player is SkullHost) return;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HostBase host = PlayerController.Instance.HostBase;
        if (collision.gameObject==host.gameObject)
        {
            host.Damage(10f);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHp -= damageAmount;
        deceleration += 0.1f;
        if(deceleration > 0.8f)
        {
            deceleration = 0.8f;
        }
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heel(int amount)
    {
        throw new System.NotImplementedException();
    }
}
