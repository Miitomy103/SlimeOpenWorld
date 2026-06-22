using UnityEngine;

[System.Serializable]
public class Move 
{
    private Vector3 currentVelocity = Vector3.zero;
    Vector3 velocityRef = Vector3.zero;

    Transform transform;
    CharacterController controller;

    public Move(Transform transform,CharacterController controller)
    {
        this.transform = transform;
        this.controller = controller;
    }
    public void Movement(Vector3 moveInput, float targetSpeed, float smoothTime, float rotationSpeed)
    {
        Camera mainCamera = Camera.main;

        // カメラ基準の入力方向を算出
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDir = (camForward * moveInput.z + camRight * moveInput.x).normalized;

        // 入力の強さで速度を調整
        float inputMag = Mathf.Clamp01(moveInput.magnitude);

        // 向きを滑らかに回転
        if (targetDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // 現在の向きを基準に前進方向を決定（見た目の方向と一致）
        Vector3 moveDir = transform.forward * (targetSpeed * inputMag);

        // 慣性を持たせて移動速度をスムーズに変化
        currentVelocity = Vector3.SmoothDamp(currentVelocity, moveDir, ref velocityRef, smoothTime);

        if (controller.enabled && controller.gameObject.activeInHierarchy)
        {
            // 実際に移動（CharacterController 用）
            controller.Move(currentVelocity * Time.deltaTime);
        }
    }
}
