using UnityEngine;

public interface IEnemy:IDamageable
{
    Transform Transform { get; }
    int CurrentHealth { get; }
    bool isDead { get; }
    void Heel(int amount);
    string EnemyId { get; }
}