using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// 検知範囲
/// </summary>
public class DetectRange : IGizmo
{
    [Header("検知設定")]
    [Tooltip("検知する範囲の半径")]
    public float detectRadius = 5f;     // 検知範囲
    [Tooltip("検知する視界の角度")]
    public float detectAngle = 90f;     // 視界の角度
    [Tooltip("検知する向きが基準のTransform")]
    public Transform angleTransform;    // 向きを基準にするTransform（例：カメラ）
    [Tooltip("検知する位置が基準のTransform")]
    public Transform positionTransform; // 位置を基準にするTransform（例：プレイヤー本体）

    /// <summary>
    /// 除外するオブジェクト
    /// </summary>
    public GameObject[] exclusions;

    [Header("Gizmo設定")]
    [SerializeField] Color lineColor = Color.red;


    public void Initialize(Transform position, Transform angle)
    {
        positionTransform = position;
        angleTransform = angle;
    }

    /// <summary>
    /// 最も近いオブジェクトを検知する
    /// </summary>
    public T DetectComponent<T>() where T : class
    {
        if (angleTransform == null || positionTransform == null)
        {
            Debug.LogWarning($" angleTransform または positionTransform が未設定です。");
            return null;
        }

        Collider[] hits = Physics.OverlapSphere(positionTransform.position, detectRadius);

        T nearest = null;
        float nearestDist = Mathf.Infinity;

        Vector3 forward = angleTransform.forward;
        forward.y = 0; // 水平方向のみ考慮する場合（不要なら削除）

        foreach (var hit in hits)
        {
            if (IsExclusion(hit.gameObject))
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

    /// <summary>
    /// 範囲内のオブジェクトをすべて検知する
    /// </summary>
    public T[] DetectComponents<T>() where T : class
    {
        if (angleTransform == null || positionTransform == null)
        {
            Debug.LogError($"angleTransform または positionTransform が未設定です。");
            return null;
        }

        Collider[] hits = Physics.OverlapSphere(positionTransform.position, detectRadius);

        List<T> detectedList = new List<T>();

        Vector3 forward = angleTransform.forward;
        forward.y = 0; // 水平方向のみ考慮する場合（不要なら削除）

        foreach (var hit in hits)
        {
            if (IsExclusion(hit.gameObject))
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

    bool IsExclusion(GameObject obj)
    {
        if (exclusions == null)
            return false;
        foreach (var exclusion in exclusions)
        {
            if (obj == exclusion)
                return true;
        }
        return false;
    }

    public void OnDrawGizmos()
    {
        if (angleTransform == null || positionTransform == null)
            return;

        Vector3 origin = positionTransform.position;
        Vector3 forward = angleTransform.forward;
        forward.y = 0;

        // 中心線
        Gizmos.color = lineColor;
        Gizmos.DrawRay(origin, forward * detectRadius);

        // 視界角度
        Vector3 rightDir = Quaternion.Euler(0, detectAngle * 0.5f, 0) * forward;
        Vector3 leftDir = Quaternion.Euler(0, -detectAngle * 0.5f, 0) * forward;

        Gizmos.color = lineColor;
        Gizmos.DrawRay(origin, rightDir * detectRadius);
        Gizmos.DrawRay(origin, leftDir * detectRadius);
        DrawArc(origin, forward, detectRadius, detectAngle, lineColor);
    }
    private void DrawArc(Vector3 center, Vector3 forward, float radius, float angle, Color color, int segments = 30)
    {
        Gizmos.color = color;

        float startAngle = -angle * 0.5f;
        float endAngle = angle * 0.5f;
        float angleStep = (endAngle - startAngle) / segments;

        Vector3 prevPoint = center + Quaternion.Euler(0, startAngle, 0) * forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Vector3 currentPoint = center + Quaternion.Euler(0, currentAngle, 0) * forward * radius;
            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }
    }
}
