using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// オブジェクトを放物線で飛ばすコンポーネント
/// </summary>
public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] float distanceTarget = 0.5f;
    [SerializeField] float duration = 0.5f;   // 飛ぶ時間
    [SerializeField] float arcHeight = 5f;    // 弧の高さ

    Action jumpEndCallback;

    Transform target;
    Vector3 startPos;
    float timer;
    bool isJumping;

    [SerializeField] UnityEvent onJumpEnd;

    private void OnEnable()
    {
            onJumpEnd?.Invoke();
    }
    void Update()
    {
        if (!isJumping) return;

        timer += Time.unscaledDeltaTime;
        float t = timer / duration;

        if (t >= 1f)
        {
            transform.position = target.position;
            isJumping = false;

            jumpEndCallback?.Invoke();
            jumpEndCallback = null;
            return;
        }

        // 水平方向補間
        Vector3 currentPos = Vector3.Lerp(startPos, target.position, t);

        // 放物線（sinで山を作る）
        float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

        currentPos.y += height;

        transform.position = currentPos;
    }

    public void Jump(Transform target, Action jumpEnd)
    {
        this.target = target;
        this.jumpEndCallback = jumpEnd;

        startPos = transform.position;
        timer = 0f;
        isJumping = true;
    }
}
