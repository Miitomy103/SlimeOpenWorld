using System.Collections;
using UnityEngine;

public class BezierMover : PoolObject
{
    [SerializeField] float duration = 1.0f;        // 飛行時間（秒）
    [SerializeField] float curveHeight = 2.0f;     // 弧の高さ
    [SerializeField] AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1); // 速度制御用（必要なら）

    public Transform endPoint;

    [Header("Looping")]
    [SerializeField]bool isLooping = false;
    [SerializeField]float loopDelay = 1.0f;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 control1;
    private Vector3 control2;
    private float time;

    private bool isFlying = false;

    private void Start()
    {
        if (endPoint != null)
        {
            StartEffect(transform.position, endPoint.position);
        }
    }

    public IEnumerator StartEffect(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;

        // start→end の方向ベクトル
        Vector3 forward = (end - start).normalized;

        // その方向に垂直な基準ベクトルを求める
        Vector3 up = Vector3.up;
        if (Vector3.Dot(forward, up) > 0.9f)  // ほぼ上向きの場合は別の基準を使う
            up = Vector3.right;

        // 垂直平面上の2軸を作る
        Vector3 right = Vector3.Cross(forward, up).normalized;
        Vector3 upDir = Vector3.Cross(right, forward).normalized;

        // 垂直平面上でランダム方向を作る
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 randomVector = (right * Mathf.Cos(angle) + upDir * Mathf.Sin(angle)) * curveHeight;

        // 中間点と制御点を設定
        Vector3 mid = (start + end) / 2f;
        Vector3 offset = randomVector;

        control1 = Vector3.Lerp(start, mid, 0.3f) + offset;
        control2 = Vector3.Lerp(mid, end, 0.7f) + offset;

        time = 0f;
        isFlying = true;

        yield return new WaitUntil(() => !isFlying);
    }


    IEnumerator LoopDelay()
    {
        if(!isLooping) yield break;
        yield return null;
        yield return new WaitForSeconds(loopDelay);
        StartEffect(startPos, endPoint.transform.position);
    }

    void Update()
    {
        if (!isFlying) return;

        time += Time.deltaTime / duration;
        float t = Mathf.Clamp01(time);
        float curvedT = speedCurve.Evaluate(t); // 加速・減速したいとき

        transform.position = GetCubicBezierPoint(curvedT, startPos, control1, control2, endPos);

        // 進行方向を向く（回転）
        if (t < 1f)
        {
            Vector3 nextPos = GetCubicBezierPoint(curvedT + 0.01f, startPos, control1, control2, endPos);
            transform.LookAt(nextPos);
        }
        else
        {
            isFlying = false;
            if (isLooping) StartCoroutine(LoopDelay());
            else gameObject.SetActive(false);
        }
    }

    private Vector3 GetCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }
}
