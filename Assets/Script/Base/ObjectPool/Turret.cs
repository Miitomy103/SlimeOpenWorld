using UnityEngine;

    public class Turret : MonoBehaviour
    {
        [SerializeField]
        string fireInputName = "Fire1";

        [SerializeField]
        Bullet bulletPrefab;

        [SerializeField]
        float shootPower = 10.0f;

        [SerializeField]
        int poolCount = 100;

        [SerializeField]
        IObjectPool<Bullet> bulletPool;

        void Start()
        {
            bulletPool = new ObjectPool<Bullet>(bulletPrefab, poolCount);
        }

        void Update()
        {
            RotateToTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetButtonDown(fireInputName))
            {
                Fire(transform.right);
            }
        }

        void RotateToTarget(Vector2 targetPoint)
        {
            var targetDirection = new Vector2(targetPoint.x - transform.position.x, targetPoint.y - transform.position.y);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg);
        }

        void Fire(Vector2 shootDirection)
        {
            var blt = bulletPool.EnableToPoolObject();
            blt.transform.position = transform.position;
            blt.transform.rotation = transform.rotation;
            blt.Fire(shootDirection, shootPower);
        }
    }