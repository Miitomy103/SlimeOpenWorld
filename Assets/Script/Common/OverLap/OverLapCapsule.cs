using UnityEngine;

/// <summary>
/// カプセルの形をOverLapで取得するクラス。
/// </summary>
public class OverLapCapsule : OverlapBase
{
    public float radius = 1f;
    public float height = 1f;
    public Vector3 center;
    public Direction direction = Direction.YAxis;
    public override T[] OverlapAll<T>(string layerName="Default")
    {
        Quaternion rotation = Quaternion.identity;
        switch (direction)
        {
            case Direction.YAxis:
                rotation = Quaternion.Euler(90f, 0f, 0f);
                break;
            case Direction.ZAxis:
                rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case Direction.XAxis:
                rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
        }
        Collider[] colliders = Physics.OverlapCapsule(transform.TransformPoint(center + GetDirectionVector() * (height / 2 - radius)),
            transform.TransformPoint(center - GetDirectionVector() * (height / 2 - radius)),
            radius);
        System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
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
        Vector3 point1 = center + GetDirectionVector() * (height / 2 - radius);
        Vector3 point2 = center - GetDirectionVector() * (height / 2 - radius);
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1 + GetDirectionVector().normalized * radius, point2 + GetDirectionVector().normalized * radius);
        Gizmos.DrawLine(point1 - GetDirectionVector().normalized * radius, point2 - GetDirectionVector().normalized * radius);
        Vector3 sideDir1, sideDir2;
        if (direction == Direction.YAxis)
        {
            sideDir1 = Vector3.forward;
            sideDir2 = Vector3.right;
        }
        else if (direction == Direction.XAxis)
        {
            sideDir1 = Vector3.up;
            sideDir2 = Vector3.forward;
        }
        else
        {
            sideDir1 = Vector3.up;
            sideDir2 = Vector3.right;
        }
        Gizmos.DrawLine(point1 + sideDir1.normalized * radius, point2 + sideDir1.normalized * radius);
        Gizmos.DrawLine(point1 - sideDir1.normalized * radius, point2 - sideDir1.normalized * radius);
        Gizmos.DrawLine(point1 + sideDir2.normalized * radius, point2 + sideDir2.normalized * radius);
        Gizmos.DrawLine(point1 - sideDir2.normalized * radius, point2 - sideDir2.normalized * radius);
    }

    private Vector3 GetDirectionVector()
    {
        switch (direction)
        {
            case Direction.YAxis:
                return Vector3.up;
            case Direction.ZAxis:
                return Vector3.forward;
            case Direction.XAxis:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }
}
