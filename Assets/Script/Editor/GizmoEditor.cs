using UnityEditor;
using UnityEngine;

/// <summary>
/// IGizmoを実装した任意のオブジェクトに対して、選択/アクティブ時にOnDrawGizmosを呼び出す共通エディタ拡張。
/// </summary>
[CustomEditor(typeof(IGizmo), true)]
public class GizmoEditor : Editor
{
    /// <summary>
    /// Unityが選択/アクティブなオブジェクトに対して自動的に呼び出すギズモ描画コールバック。
    /// </summary>
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawCustomGizmo( GizmoType gizmoType)
    {
        IGizmo gizmoTarget = (IGizmo)Selection.activeObject;
        gizmoTarget.OnDrawGizmos();
    }
}
