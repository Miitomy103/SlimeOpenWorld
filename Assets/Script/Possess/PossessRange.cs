using System.Collections.Generic;
using UnityEngine;

public class PossessRange : MonoBehaviour
{
    [Header("検知設定")]
    [SerializeField]public float detectRadius = 5f;     // 検知範囲
    [SerializeField]public float detectAngle = 60f;     // 視界の角度

    [Header("参照設定")]
    [SerializeField]public Transform angleTransform;    // 向きを基準にするTransform（例：カメラ）
    [SerializeField]public Transform positionTransform; // 位置を基準にするTransform（例：プレイヤー本体）

    public T DetectEnemy<T>() where T : class
    {
        if (angleTransform == null || positionTransform == null)
        {
            Debug.LogWarning($"{nameof(PossessRange)}: angleTransform または positionTransform が未設定です。");
            return null;
        }

        Collider[] hits = Physics.OverlapSphere(positionTransform.position, detectRadius);

        T nearest = null;
        float nearestDist = Mathf.Infinity;

        Vector3 forward = angleTransform.forward;
        forward.y = 0; // 水平方向のみ考慮する場合（不要なら削除）

        foreach (var hit in hits)
        {
            if (hit.gameObject==gameObject)
                continue;

            T comp = hit.GetComponent<T>();
            if (comp == null)
                continue;

            Vector3 dirToTarget = (hit.transform.position - positionTransform.position).normalized;
            float angle = Vector3.Angle(forward, dirToTarget);

            if (angle < detectAngle * 0.5f)
            {
                float dist = Vector3.Distance(positionTransform.position, hit.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = comp;
                }
            }
        }

        return nearest;
    }
    public T[] DetectEnemies<T>() where T : class
    {
        if (angleTransform == null || positionTransform == null)
        {
            Debug.LogWarning($"{nameof(PossessRange)}: angleTransform または positionTransform が未設定です。");
            return null;
        }

        Collider[] hits = Physics.OverlapSphere(positionTransform.position, detectRadius);

        List<T> detectedList = new List<T>();

        Vector3 forward = angleTransform.forward;
        forward.y = 0; // 水平方向のみ考慮する場合（不要なら削除）

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            T comp = hit.GetComponent<T>();
            if (comp == null)
                continue;

            Vector3 dirToTarget = (hit.transform.position - positionTransform.position).normalized;
            float angle = Vector3.Angle(forward, dirToTarget);

            if (angle < detectAngle * 0.5f)
            {
                detectedList.Add(comp);
            }
        }

        return detectedList.ToArray();
    }

    void OnDrawGizmosSelected()
    {
        if (angleTransform == null || positionTransform == null)
            return;

        Vector3 origin = positionTransform.position;
        Vector3 forward = angleTransform.forward;
        forward.y = 0;

        Gizmos.color = new Color(1f, 0.6f, 0f, 0.4f);
        Gizmos.DrawWireSphere(origin, detectRadius);

        // 中心線
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, forward * detectRadius);

        // 視界角度
        Vector3 rightDir = Quaternion.Euler(0, detectAngle * 0.5f, 0) * forward;
        Vector3 leftDir = Quaternion.Euler(0, -detectAngle * 0.5f, 0) * forward;

        Gizmos.color = new Color(1f, 0.6f, 0f, 0.4f);
        Gizmos.DrawRay(origin, rightDir * detectRadius);
        Gizmos.DrawRay(origin, leftDir * detectRadius);
    }
}
