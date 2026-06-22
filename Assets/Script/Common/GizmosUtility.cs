using UnityEngine;

/// <summary>
/// Gizmosのユーティリティ
/// </summary>
public static class GizmosUtility
{
    private static int _circleVertexCount = 64;

    /// <summary>
    /// 円を描く(2D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="radius">半径</param>
    public static void DrawWireCircle(Vector3 center, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// 正多角形を描く(2D)
    /// </summary>
    /// <param name="vertexCount">角の数</param>
    /// <param name="center">中心位置</param>
    /// <param name="radius">半径</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, float radius)
    {
        DrawWireRegularPolygon(vertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// 円を描く(3D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="rotation">回転</param>
    /// <param name="radius">半径</param>
    public static void DrawWireCircle(Vector3 center, Quaternion rotation, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, rotation, radius);
    }

    /// <summary>
    /// 正多角形を描く(3D)
    /// </summary>
    /// <param name="vertexCount">角の数</param>
    /// <param name="center">中心位置</param>
    /// <param name="rotation">回転</param>
    /// <param name="radius">半径</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, Quaternion rotation, float radius)
    {
        if (vertexCount < 3)
        {
            return;
        }

        Vector3 previousPosition = Vector3.zero;
        float step = 2f * Mathf.PI / vertexCount;
        float offset = Mathf.PI * 0.5f + ((vertexCount % 2 == 0) ? step * 0.5f : 0f);

        for (int i = 0; i <= vertexCount; i++)
        {
            float theta = step * i + offset;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 nextPosition = center + rotation * new Vector3(x, y, 0f);

            if (i == 0)
            {
                previousPosition = nextPosition;
            }
            else
            {
                Gizmos.DrawLine(previousPosition, nextPosition);
            }

            previousPosition = nextPosition;
        }
    }

    /// <summary>
    /// 長方形を描く(2D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="size">サイズ（幅・高さ）</param>
    public static void DrawWireRectangle(Vector3 center, Vector2 size)
    {
        DrawWireRectangle(center, Quaternion.identity, size);
    }

    /// <summary>
    /// 長方形を描く(3D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="rotation">回転</param>
    /// <param name="size">サイズ（幅・高さ）</param>
    public static void DrawWireRectangle(Vector3 center, Quaternion rotation, Vector2 size)
    {
        Vector3 halfSize = new Vector3(size.x * 0.5f, size.y * 0.5f, 0f);

        Vector3[] corners = new Vector3[4];
        corners[0] = center + rotation * new Vector3(-halfSize.x, -halfSize.y, 0f);
        corners[1] = center + rotation * new Vector3(halfSize.x, -halfSize.y, 0f);
        corners[2] = center + rotation * new Vector3(halfSize.x, halfSize.y, 0f);
        corners[3] = center + rotation * new Vector3(-halfSize.x, halfSize.y, 0f);

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}
