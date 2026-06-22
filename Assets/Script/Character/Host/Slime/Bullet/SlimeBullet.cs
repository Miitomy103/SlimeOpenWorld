using UnityEngine;

/// <summary>
/// スライムが発射する弾。一定時間または衝突でプールに戻る
/// </summary>
public class SlimeBullet : PoolObject
{
    float speed = 10f;
    float lifeTime = 3f;
    float timer;
    int power = 1;
    void Update()
    {
        // 前方に移動
        transform.position += transform.forward * speed * Time.deltaTime;
        // 生存時間のカウント
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            DoDisable();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IDamageable>(out var damagable))
        {
            // ダメージを与える
            damagable.TakeDamage(power);
            // 他のオブジェクトに衝突したら無効化
            DoDisable();
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // 他のオブジェクトに衝突したら無効化
            DoDisable();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damagable))
        {
            // ダメージを与える
            damagable.TakeDamage(power);
            // 他のオブジェクトに衝突したら無効化
            DoDisable();
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // 他のオブジェクトに衝突したら無効化
            DoDisable();
        }
    }

    public override void DoDisable()
    {
        base.DoDisable();
        timer = 0f;
    }

    public void Fire(Vector3 pos,Quaternion qua,float speed,float lifeTime,int power)
    {
        this.speed = speed;
        this.lifeTime = lifeTime;
        transform.position = pos;
        transform.rotation = qua;
        this.power = power;
    }
}
