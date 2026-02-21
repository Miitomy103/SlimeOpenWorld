using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// オープンワールド風ミニマップ / フルマップ管理クラス
/// </summary>
public class MapManager : MonoBehaviour, IDragHandler, IScrollHandler, IPointerClickHandler
{
    [Header("=== マップ設定 ===")]
    [Tooltip("マップ全体の RawImage コンポーネント")]
    public RawImage mapImage;

    [Tooltip("ワールド全体サイズ (メートル単位)")]
    public Vector2 worldSize = new Vector2(2000f, 2000f);

    [Tooltip("ワールド原点オフセット (左下を (0,0) にしたい場合は worldSize/2)")]
    public Vector2 worldOffset = new Vector2(1000f, 1000f);

    [Header("=== ズーム設定 ===")]
    [Range(0.5f, 1f)]  public float zoomMin  = 0.05f;
    [Range(1f,  20f)]  public float zoomMax  = 4f;
    [Range(0.01f, 1f)] public float zoomStep = 0.15f;
    [Range(1f, 20f)]   public float zoomSmoothSpeed = 8f;

    [Header("=== パン設定 ===")]
    [Range(0.1f, 5f)] public float panSmoothSpeed = 10f;

    [Header("=== プレイヤー ===")]
    [Tooltip("プレイヤー Transform (実ワールド空間)")]
    public Transform playerTransform=>PlayerController.Instance.HostBase.transform;

    [Tooltip("マップ上のプレイヤーアイコン")]
    public RectTransform playerMarker;

    [Tooltip("プレイヤーアイコンを常に中央に追従させるか")]
    public bool followPlayer = true;

    [Header("=== アイコン管理 ===")]
    [Tooltip("MapIcon プレハブ (MapIcon.cs をアタッチ済み)")]
    public MapIcon iconPrefab;

    [Tooltip("アイコンを配置する親 Transform (MapRoot 直下推奨)")]
    public RectTransform iconContainer;

    // ── 内部状態 ──────────────────────────────────────────────────
    private RectTransform _mapRect;
    private Vector2       _uvOffset;         // 現在の UV オフセット
    private Vector2       _uvOffsetTarget;   // スムーズ用目標
    private float         _uvScale = 1f;     // ズームスケール (UV空間)
    private float         _uvScaleTarget = 1f;

    private readonly List<MapIcon> _icons = new List<MapIcon>();

    // ── Unity ライフサイクル ───────────────────────────────────────
    void Awake()
    {
        _mapRect = mapImage.rectTransform;
        _uvOffset = Vector2.zero;
        _uvOffsetTarget = Vector2.zero;
        ApplyUV();
        gameObject.SetActive(false); 
    }

    void Update()
    {
        // プレイヤー追従
        if (followPlayer && playerTransform != null)
        {
            Vector2 playerUV = WorldToUV(new Vector2(playerTransform.position.x,
                                                     playerTransform.position.z));
            _uvOffsetTarget = playerUV - new Vector2(0.5f, 0.5f) * _uvScale;
        }

        // スムーズ補間
        _uvOffset = Vector2.Lerp(_uvOffset, _uvOffsetTarget, Time.deltaTime * panSmoothSpeed);
        _uvScale  = Mathf.Lerp(_uvScale, _uvScaleTarget, Time.deltaTime * zoomSmoothSpeed);

        ClampOffset();
        ApplyUV();

        // プレイヤーマーカー位置更新
        if (playerMarker != null && playerTransform != null)
        {
            playerMarker.anchoredPosition = UVToMapPos(
                WorldToUV(new Vector2(playerTransform.position.x,
                                     playerTransform.position.z)));
        }

        // 全アイコン位置更新
        foreach (var icon in _icons)
            icon.UpdatePosition(this);
    }

    // ── IEventSystem 実装 ─────────────────────────────────────────

    /// <summary>マウス / タッチドラッグでパン</summary>
    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ量を UV 空間に変換
        Vector2 mapSizePx = _mapRect.rect.size;
        Vector2 delta = eventData.delta / mapSizePx * _uvScale;
        _uvOffsetTarget -= delta;
        // ドラッグ中は追従を一時停止
        followPlayer = false;
    }

    /// <summary>マウスホイールでズーム</summary>
    public void OnScroll(PointerEventData eventData)
    {
        float scroll = eventData.scrollDelta.y;
        ZoomAtPoint(eventData.position, -scroll * zoomStep);
    }

    /// <summary>ダブルクリックでアイコン追加 / 追従再開</summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Vector2 uv = MapPosToUV(eventData.position);
            Vector2 world = UVToWorld(uv);
            // MapUI 経由でアイコン種別を選択するためイベント通知
            MapEvents.OnMapDoubleClick?.Invoke(world, uv);
        }
    }

    // ── 公開 API ─────────────────────────────────────────────────

    /// <summary>ワールド座標にアイコンを追加する</summary>
    public MapIcon AddIcon(Vector2 worldPos, MapIconType type, string label = "")
    {
        if (iconPrefab == null || iconContainer == null)
        {
            Debug.LogWarning("[MapManager] iconPrefab または iconContainer が未設定です");
            return null;
        }
        var icon = Instantiate(iconPrefab, iconContainer);
        icon.Initialize(worldPos, type, label, this);
        _icons.Add(icon);
        return icon;
    }

    /// <summary>アイコンを削除する</summary>
    public void RemoveIcon(MapIcon icon)
    {
        if (_icons.Remove(icon))
            Destroy(icon.gameObject);
    }

    /// <summary>全アイコンを削除する</summary>
    public void ClearAllIcons()
    {
        foreach (var icon in _icons)
            Destroy(icon.gameObject);
        _icons.Clear();
    }

    /// <summary>指定ズームレベルにアニメーション</summary>
    public void SetZoom(float zoom) =>
        _uvScaleTarget = Mathf.Clamp(1f / zoom, 1f / zoomMax, 1f / zoomMin);

    /// <summary>プレイヤー追従を再開</summary>
    public void ResumeFollowPlayer() => followPlayer = true;

    // ── 座標変換ユーティリティ (公開) ────────────────────────────

    /// <summary>ワールド XZ → UV [0,1]</summary>
    public Vector2 WorldToUV(Vector2 worldXZ)
    {
        return new Vector2(
            (worldXZ.x + worldOffset.x) / worldSize.x,
            (worldXZ.y + worldOffset.y) / worldSize.y
        );
    }

    /// <summary>UV → ワールド XZ</summary>
    public Vector2 UVToWorld(Vector2 uv)
    {
        return new Vector2(
            uv.x * worldSize.x - worldOffset.x,
            uv.y * worldSize.y - worldOffset.y
        );
    }

    /// <summary>UV → マップ RectTransform 上のアンカー位置 (px)</summary>
    public Vector2 UVToMapPos(Vector2 uv)
    {
        Vector2 size = _mapRect.rect.size;
        float nx = (uv.x - _uvOffset.x) / _uvScale;
        float ny = (uv.y - _uvOffset.y) / _uvScale;
        return new Vector2((nx - 0.5f) * size.x, (ny - 0.5f) * size.y);
    }

    // ── 内部ヘルパー ─────────────────────────────────────────────

    private void ZoomAtPoint(Vector2 screenPos, float delta)
    {
        float newScale = Mathf.Clamp(_uvScaleTarget + delta * _uvScaleTarget,
                                     1f / zoomMax, 1f / zoomMin);
        // ズーム中心を維持するためオフセット補正
        Vector2 uvBefore = MapPosToUV(screenPos);
        float ratio = newScale / _uvScaleTarget;
        _uvOffsetTarget = uvBefore - (uvBefore - _uvOffsetTarget) * ratio;
        _uvScaleTarget  = newScale;
    }

    private Vector2 MapPosToUV(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _mapRect, screenPos, null, out Vector2 local);
        Vector2 size = _mapRect.rect.size;
        float nx = local.x / size.x + 0.5f;
        float ny = local.y / size.y + 0.5f;
        return new Vector2(nx * _uvScale + _uvOffset.x,
                           ny * _uvScale + _uvOffset.y);
    }

    private void ClampOffset()
    {
        float s = _uvScale;
        _uvOffset.x = Mathf.Clamp(_uvOffset.x, 0f, 1f - s);
        _uvOffset.y = Mathf.Clamp(_uvOffset.y, 0f, 1f - s);
        _uvOffsetTarget.x = Mathf.Clamp(_uvOffsetTarget.x, 0f, 1f - s);
        _uvOffsetTarget.y = Mathf.Clamp(_uvOffsetTarget.y, 0f, 1f - s);
    }

    private void ApplyUV()
    {
        mapImage.uvRect = new Rect(_uvOffset.x, _uvOffset.y, _uvScale, _uvScale);

        // アイコンの可視性をマップ範囲に合わせてクリップ
        foreach (var icon in _icons)
            icon.SetVisible(IsUVVisible(icon.UV));
    }

    private bool IsUVVisible(Vector2 uv)
    {
        return uv.x >= _uvOffset.x && uv.x <= _uvOffset.x + _uvScale &&
               uv.y >= _uvOffset.y && uv.y <= _uvOffset.y + _uvScale;
    }
}

/// <summary>マップイベント (静的デリゲート)</summary>
public static partial class MapEvents
{
    public static System.Action<Vector2, Vector2> OnMapDoubleClick;
}
