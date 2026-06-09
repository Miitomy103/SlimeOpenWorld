using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 球の形をOverLapで取得するクラス。
/// </summary>
public class OverLapSphere : OverlapBase
{
    public float radius = 1f;
    public Vector3 center;
    public override T[] OverlapAll<T>(string layerName = "Default")
    {
        Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(center), radius, LayerMask.GetMask(layerName), QueryTriggerInteraction.Ignore);
        List<T> results = new List<T>();
        foreach (var collider in colliders)
        {
            T component = collider.GetComponent<T>();
            if (component != null)
            {
                results.Add(component);
            }
        }
        return results.ToArray();
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(center, radius);
    }
}
