using UnityEngine;
using InGame;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class SlimeAttack : IState<SlimeHost>
{
    [SerializeField] float attackDuration = 2f;
    [SerializeField] float trackingDistance = 5f;

    [Header("UŒ‚—Í")]
    [SerializeField] int attackPower = 10;

    [SerializeField] OverlapBase detector;

    AnimAttackAcyncFrame attackCoroutine;

    [SerializeField] AnimAttackAcyncFrameSO attackData;
    [SerializeField] bool isAttack;

    [SerializeField] UnityEvent onAttackStart;
    public void DoExit(SlimeHost owner)
    {

    }

    public void DoStart(SlimeHost owner)
    {
        EnemyBase enemy = DetectEnemy(owner);
        attackCoroutine = new AnimAttackAcyncFrame(attackData);

        if (enemy != null)
        {
            Vector3 direction = (enemy.transform.position - owner.transform.position).normalized;
            direction.y = 0; // Keep only horizontal direction
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                owner.transform.rotation = lookRotation;
            }
        }

        owner.animator.SetTrigger("Attack");

        attackCoroutine.AttackCoroutineAsync(
            enabled: () => AttackTrue(),
            disabled: () => AttackFalse()
            ).Forget();

        WaitForAttackEnd().ContinueWith(() =>
        {
            owner.stateMachine.ChangeState(owner.SlimeIdle);
        });

        onAttackStart?.Invoke();
    }

    UniTask WaitForAttackEnd()
    {
        return UniTask.Delay(System.TimeSpan.FromSeconds(attackDuration));
    }

    EnemyBase DetectEnemy(SlimeHost owner)
    {
        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, trackingDistance);
        foreach (var hitCollider in hitColliders)
        {
            EnemyBase enemy = hitCollider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                return enemy;
            }
        }
        return null;
    }

    void AttackTrue()
    {
        isAttack = true;
    }
    void AttackFalse()
    {
        isAttack = false;
        hitEnemyList.Clear();
    }

    List<EnemyBase> hitEnemyList = new List<EnemyBase>();
    public void DoUpdate(SlimeHost owner)
    {
        if(isAttack)
        {
            EnemyBase[] enemies = detector.OverlapAll<EnemyBase>();
            Debug.Log($"Detected {enemies.Length} enemies.");
            foreach (var enemy in enemies)
            {
                if (!hitEnemyList.Contains(enemy))
                {
                    enemy.TakeDamage(attackPower);
                    hitEnemyList.Add(enemy);
                }
            }
        }
    }

}
