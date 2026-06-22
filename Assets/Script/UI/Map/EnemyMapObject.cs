using UnityEngine;

/// <summary>
/// ミニマップ上で敵の位置を示すアイコンを管理するクラス。
/// </summary>
[System.Serializable]
public class EnemyMapObject : IMapObject
{
    [SerializeField] Transform enemy;
    public Transform Enemy => enemy;
    public RectTransform Icon { get; set; }
    [SerializeField] RectTransform iconPrefab;
    public void Enable(Transform c)
    {
        Icon = GameObject.Instantiate(iconPrefab,c);
    }
    public void Disable()
    {
        if (Icon != null)
        {
            GameObject.Destroy(Icon.gameObject);
        }
    }
    public void UpdateMapObject(Camera miniMapCamera, RectTransform mapBounds)
    {
        if (!Enemy) return;

        // 1. カメラからの相対位置を取得
        Vector3 relative = Enemy.position - miniMapCamera.transform.position;

        // --- ここを追加・変更 ---
        // 2. カメラのY軸回転の逆向きにベクトルを回転させる
        // これにより、カメラの正面方向が(0, 0, 1)になるように座標変換されます
        Quaternion camRotation = Quaternion.Euler(0, -miniMapCamera.transform.eulerAngles.y, 0);
        Vector3 rotatedRelative = camRotation * relative;
        // -----------------------

        float camHeight = miniMapCamera.orthographicSize * 2f;
        float camWidth = camHeight * miniMapCamera.aspect;

        float mapWidth = mapBounds.rect.width;
        float mapHeight = mapBounds.rect.height;

        // 3. 回転済みのベクトル（rotatedRelative）を使って座標を計算
        float x = rotatedRelative.x / camWidth * mapWidth;
        float y = rotatedRelative.z / camHeight * mapHeight;

        Vector2 pos = new Vector2(x, y);

        // アイコンサイズ考慮
        Vector2 iconHalf = Icon.rect.size * 0.5f;

        float minX = -mapWidth / 2 + iconHalf.x;
        float maxX = mapWidth / 2 - iconHalf.x;
        float minY = -mapHeight / 2 + iconHalf.y;
        float maxY = mapHeight / 2 - iconHalf.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        Icon.anchoredPosition = pos;
    }

}
