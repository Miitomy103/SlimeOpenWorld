using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IGizmo), true)]
public class GizmoEditor : Editor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawCustomGizmo( GizmoType gizmoType)
    {
        IGizmo gizmoTarget = (IGizmo)Selection.activeObject;
        gizmoTarget.OnDrawGizmos();
    }
}
