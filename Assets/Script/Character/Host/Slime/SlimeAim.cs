using UnityEngine;
using InGame;

/// <summary>
/// スライムが弾を撃つステート
/// </summary>
[System.Serializable]
public class SlimeAim :IState<SlimeHost>
{
    [SerializeField]bool isAiming = false;

    [SerializeField] float speed = 2f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float aimSpeed = 0.2f;
    [SerializeField] float trackingDistance = 20f;

    float yaw;   // Y軸（左右）
    float pitch; // X軸（上下）+
    [SerializeField] float minPitch = -60f;
    [SerializeField] float maxPitch = 60f;


    [Header("Bullet")]
    [SerializeField] SlimeBullet bulletPrefab;

    [SerializeField]Vector3 fireOffset = new Vector3(0, -1, 0);
    [SerializeField]float attackCooldown = 1f;
    [SerializeField]float bulletSpeed = 10f;
    [SerializeField] float bulletLifeTime = 3f;

    [SerializeField]int minPower = 1;
    ObjectPool<SlimeBullet> objectPool;
    IObjectPool<SlimeBullet> bulletPool => objectPool;

    [SerializeField] UnityEngine.Events.UnityEvent onBulletAttack;

    public void DoExit(SlimeHost owner)
    {

    }

    public void DoStart(SlimeHost owner)
    {
        if(!isAiming)
        {
            owner.stateMachine.ChangeState(owner.SlimeIdle);
            return;
        }
        objectPool=new ObjectPool<SlimeBullet>(bulletPrefab,1);
    }

    public void DoUpdate(SlimeHost owner)
    {
        if(owner.Input.Action0.onDown)
        {
            Attack(owner);
        }
        CheckCondition(owner);
        HandleInput(owner);
    }
    void CheckCondition(SlimeHost owner)
    {
        if(owner.Input.Action1.onUp)
        {
            Debug.Log("Aim Release");
            owner.stateMachine.ChangeState(owner.SlimeIdle);
        }
    }

    void Attack(SlimeHost owner)
    {
        var bullet = bulletPool.EnableToPoolObject();
        bullet.gameObject.SetActive(true);
        if (bullet == null) return;

        Vector3 pos = owner.transform.position + fireOffset;

        // 水平(Yaw)は owner
        float yaw = owner.transform.eulerAngles.y;

        float pitch = 0f;
        if (TryDetectEnemy(owner, out var enemy))
        {
            // 縦(Pitch)は狙い用Transform
            pitch = -enemy.Transform.eulerAngles.x;
        }
        else
        {
            pitch = owner.transform.eulerAngles.x;
        }

        // ロールは使わないなら0
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

        bullet.Fire(pos, rot, bulletSpeed, bulletLifeTime, minPower);

        onBulletAttack?.Invoke();

    }
    IEnemy DetectEnemy(SlimeHost owner)
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(owner.transform.position, trackingDistance);

        IEnemy nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            IEnemy enemy = hitCollider.GetComponent<IEnemy>();
            if (enemy == null) continue;

            float distance = Vector3.Distance(
                owner.transform.position,
                enemy.Transform.position
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
    bool TryDetectEnemy(SlimeHost owner, out IEnemy enemy)
    {
        enemy = DetectEnemy(owner);
        return enemy != null;
    }

    public void HandleInput(SlimeHost owner)
    {
        PlayerInput input = owner.Input;


        if (input.Move == Vector2.zero)
        {
            owner.animator.SetBool("IsWalk", false);
            return;
        }

        Vector3 direction= owner.movement.HandleInput(speed, input.Move, Vector3.zero);

        if(TryDetectEnemy(owner,out var enemy))
        {
            Vector3 toEnemy = enemy.Transform.position - owner.transform.position;
            toEnemy.y = 0;
            direction = toEnemy.normalized;
        }
        owner.rotator.RotateTowards(direction, rotateSpeed);

        owner.animator.SetBool("IsWalk", false);
    }
}
