using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Cinemachineカメラのワールド座標Yだけをターゲットに追従させるエクステンション。
/// </summary>
public class LookTargetYOnlyExtension : CinemachineExtension
{
    [SerializeField] Transform lookTarget;
    [SerializeField] float minOffsetY = -2f;
    [SerializeField] float maxOffsetY = 3f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        // Body計算後にYだけ差し替える
        if (stage != CinemachineCore.Stage.Body) return;
        if (lookTarget == null) return;

        Vector3 pos = state.RawPosition;

        // カメラ → ターゲット方向からYだけ取得
        float dirY = lookTarget.position.y - pos.y;

        // 好きな制限
        float offsetY = Mathf.Clamp(dirY, minOffsetY, maxOffsetY);

        pos.y += offsetY;

        state.RawPosition = pos;
    }
}
