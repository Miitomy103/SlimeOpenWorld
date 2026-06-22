
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : PoolObject
{
    public void Fire(Vector3 direction, float power)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * power;
    }
}