using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [SerializeField] float speed = 3f;

    [SerializeField]bool isWalk = false;

     Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 入力ベクトル
        Vector3 input = new Vector3(h, 0, v);

        if (input.magnitude > 0.1f)
        {
            isWalk = true;

            // カメラ基準の forward / right を取得（XZ 平面に投影）
            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // カメラ基準で移動方向を決定
            Vector3 move = camForward * v + camRight * h;

            // 向きを移動方向に合わせる
            transform.rotation = Quaternion.LookRotation(move);

            // 移動
            rb.MovePosition(transform.position + move.normalized * speed * Time.deltaTime);
        }
        else
        {
            isWalk = false;
        }
        if(animator != null)
        {
            animator.SetBool("IsWalk", isWalk);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

}
