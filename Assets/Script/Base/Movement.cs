using UnityEngine;

public class Movement
{
    readonly Transform target;
    readonly CharacterController controller;
    public Movement(Transform target, CharacterController controller)
    {
        this.target = target;
        this.controller = controller;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 HandleInput(float speed,Vector2 moveInput,Vector3 velocity)
    {
        // 入力ベクトル（ローカル基準）
        Vector3 moveInput3 = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveInput3.magnitude > 0.1f)
        {
            Vector3 camForward = Vector3.forward;
            Vector3 camRight = Vector3.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // カメラ基準で移動方向を決定
            Vector3 move = camForward * moveInput3.z + camRight * moveInput3.x;
            move.Normalize();

            // 向きを移動方向に合わせる
            //Quaternion targetRot = Quaternion.LookRotation(move);
            //target.transform.rotation = Quaternion.Slerp(
            //    target.transform.rotation,
            //    targetRot,
            //    rotateSpeed * Time.deltaTime
            //);


            if (controller != null)
            {
                // 重力を考慮（必要なら）
                velocity.y += Physics.gravity.y * Time.deltaTime;
                controller.Move((move * speed + Vector3.up * velocity.y) * Time.deltaTime);

                if (controller.isGrounded && velocity.y < 0)
                    velocity.y = -2f;

            }
            else
            {
                Debug.LogWarning("CharacterController が見つかりません。");
            }
            return move * speed;
        }
        return Vector3.zero;
    }
}
