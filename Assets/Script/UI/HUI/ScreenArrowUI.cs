using UnityEngine;

public class ScreenArrowUI : MonoBehaviour
{
    [SerializeField]RectTransform arrowRect;
    [SerializeField] Transform target;

    private void Awake()
    {
        if (arrowRect == null)
            arrowRect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        // ターゲットのスクリーン座標を取得
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

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

        // 2. 画面内かどうかの判定（マージン 50px を考慮）
        float margin = 50f;
        {
            // 矢印を表示
            arrowRect.gameObject.SetActive(true);

            // 中心からの相対ベクトルを計算
            Vector2 dir = (Vector2)screenPos - screenCenter;

            // 3. 画面端に張り付くように座標を計算（アスペクト比を考慮したクランプ）
            float m = dir.y / dir.x; // 傾き
            Vector2 clampedPos = Vector2.zero;

            // 画面の上下左右の端を判定して座標を決定
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

            // 実際のスクリーン座標に戻す
            arrowRect.position = clampedPos + screenCenter;

            // 4. 回転の設定
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowRect.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        

    }
}
