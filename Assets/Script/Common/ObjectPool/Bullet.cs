using UnityEngine;

/// <summary>
/// 指定方向へRigidbody2Dの速度を設定して発射する、プール対応の弾オブジェクト。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : PoolObject
{
    public void Fire(Vector3 direction, float power)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * power;
    }
}