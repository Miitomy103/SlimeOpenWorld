using UnityEngine;

public class ScreenArrowUI : MonoBehaviour
{
    [SerializeField] RectTransform arrowRect;
    [SerializeField] Transform target;
    [SerializeField] float margin = 50f;

    private bool isActive = true;

    private void Awake()
    {
        if (arrowRect == null)
            arrowRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!isActive || target == null)
        {
            if (arrowRect != null)
                arrowRect.gameObject.SetActive(false);
            return;
        }

        // ターゲットのスクリーン座標を取得
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position+Vector3.up*2);

        // 1. 背後判定の処理
        bool isOffScreen = false;
        if (screenPos.z < 0)
        {
            isOffScreen = true;
            // 背後にある場合は座標を反転させる
            screenPos *= -1;
        }

        // 画面の半分サイズ
        float halfWidth = Screen.width / 2f;
        float halfHeight = Screen.height / 2f;
        Vector2 screenCenter = new Vector2(halfWidth, halfHeight);

        // 矢印を表示
        arrowRect.gameObject.SetActive(true);

        // 中心からの相対ベクトルを計算
        Vector2 dir = (Vector2)screenPos - screenCenter;

        // 2. 画面内かどうかの判定（マージンを考慮）
        bool isOnScreen = !isOffScreen &&
                          screenPos.x > margin && screenPos.x < Screen.width - margin &&
                          screenPos.y > margin && screenPos.y < Screen.height - margin;

        Vector2 arrowPos;

        if (isOnScreen)
        {
            // 画面内ならターゲットのスクリーン座標に矢印を配置
            arrowPos = new Vector2(screenPos.x, screenPos.y);
        }
        else
        {
            // 3. 画面端に張り付くように座標を計算（アスペクト比を考慮したクランプ）
            float m = dir.y / dir.x; // 傾き
            Vector2 clampedPos = Vector2.zero;

            if (Mathf.Abs(dir.x) * halfHeight > Mathf.Abs(dir.y) * halfWidth)
            {
                // 左右の端に到達
                float xDist = halfWidth - margin;
                clampedPos.x = dir.x > 0 ? xDist : -xDist;
                clampedPos.y = clampedPos.x * m;
            }
            else
            {
                // 上下の端に到達
                float yDist = halfHeight - margin;
                clampedPos.y = dir.y > 0 ? yDist : -yDist;
                clampedPos.x = clampedPos.y / m;
            }

            arrowPos = clampedPos + screenCenter;
        }

        // 実際のスクリーン座標に適用
        arrowRect.position = arrowPos;

        // 4. 回転の設定（常にターゲット方向を向く）
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowRect.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    /// <summary>
    /// ターゲットを設定
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// 矢印の表示/非表示を設定
    /// </summary>
    public void SetActive(bool active)
    {
        isActive = active;
        if (!active && arrowRect != null)
        {
            arrowRect.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// マージンを設定
    /// </summary>
    public void SetMargin(float newMargin)
    {
        margin = newMargin;
    }
}