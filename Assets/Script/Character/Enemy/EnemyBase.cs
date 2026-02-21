using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField] string enemyID;
    public string EnemyId => enemyID;
    public int CurrentHealth => currentHealth;

    public bool isDead => currentHealth <= 0;

    public Vector3 defaultPosition { get; protected set; }
    public Quaternion defaultRotation { get; protected set; }

    public Transform Transform => transform;

    protected virtual void Awake()
    {
        // Base initialization logic for all enemies can go here
    }
    virtual protected void Start()
    {
        currentHealth = maxHealth;
        defaultPosition = transform.position;
    }
    public virtual void DoUpdate()
    {
        // Base update logic for all enemies can go here
    }

    public abstract void OnDetector(bool isDetect);

    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} took {damage} damage.");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameEvents.EnemyKilled(enemyID);
    }
    public void Heel(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
