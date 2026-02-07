using UnityEngine;

public class EnemyNest : MonoBehaviour
{
    [SerializeField] EnemyBase[] enemyBases;

    [SerializeField]float detectorRadius =25f;
    [SerializeField]float loadRadius = 50f;

    public bool isPlayerInLoad = false;
    public bool isPlayerInDetector = false;

    private void Update()
    {
        Transform host = PlayerController.Instance.HostBase.transform;
        if (!IsPlayerInLoadRadius(host)) return;

        bool  inPlayer = IsPlayerInDetectorRadius(host);
        foreach (var enemy in enemyBases)
        {
            enemy.DoUpdate();
        }
        if(inPlayer)
        {
            if(!isPlayerInDetector)
            {
                foreach (var enemy in enemyBases)
                {
                    enemy.OnDetector(true);
                }
                isPlayerInDetector = true;
            }
        }
        else
        {
            if (isPlayerInDetector)
            {
                foreach (var enemy in enemyBases)
                {
                    enemy.OnDetector(false);
                }
                isPlayerInDetector = false;
            }
        }

    }

    /// <summary>
    /// プレイヤーが検出範囲内にいるかどうか
    /// </summary>
    public bool IsPlayerInDetectorRadius(Transform host)
    {
        float distance = Vector3.Distance(transform.position, host.position);
        return distance <= detectorRadius;
    }
    void OnPlayerInDetector(bool onActive)
    {

    }
    /// <summary>
    /// プレイヤーがロード範囲内にいるかどうか
    /// Managerが呼ぶ
    /// </summary>
    public bool IsPlayerInLoadRadius(Transform host)
    {
        float distance = Vector3.Distance(transform.position, host.position);
        bool isInLoad = distance <= loadRadius;
        if(isInLoad != isPlayerInLoad)
        {
            isPlayerInLoad = isInLoad;
            OnPlayerInLoad(isInLoad);
        }

        return isInLoad;
    }
    void OnPlayerInLoad(bool onActive)
    {
            foreach (var enemy in enemyBases)
            {
                enemy.gameObject.SetActive(onActive);
            }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectorRadius);

        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loadRadius);
    }
}
