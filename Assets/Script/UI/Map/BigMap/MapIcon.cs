using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// マップ上のアイコン種別
/// </summary>
public enum MapIconType
{
    Custom      = 0,
    Quest       = 1,
    Enemy       = 2,
    Treasure    = 3,
    Town        = 4,
    Dungeon     = 5,
    Waypoint    = 6,
    NPC         = 7,
    FastTravel  = 8,
    Danger      = 9,
}

/// <summary>
/// マップアイコン 1個を管理するコンポーネント
/// RawImage や Image を持つ UI プレハブにアタッチする
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class MapIcon : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("=== 参照 ===")]
    public Image         iconImage;
    public Text labelText;
    public GameObject    tooltipRoot;
    public Text tooltipText;

    [Header("=== アイコン設定 ===")]
    public MapIconConfig iconConfig;   // ScriptableObject で色・スプライトを管理

    // ── プロパティ ───────────────────────────────────────────────
    public Vector2      WorldPosition { get; private set; }
    public Vector2      UV            { get; private set; }
    public MapIconType  IconType      { get; private set; }
    public string       Label         { get; private set; }

    private MapManager  _manager;
    private RectTransform _rect;
    private bool        _isHovered;

    // アニメーション用
    private float _bobTime;
    private Vector3 _baseScale;

    // ── 初期化 ──────────────────────────────────────────────────

    public void Initialize(Vector2 worldPos, MapIconType type, string label, MapManager manager)
    {
        _rect         = GetComponent<RectTransform>();
        _manager      = manager;
        WorldPosition = worldPos;
        IconType      = type;
        Label         = label;
        UV            = manager.WorldToUV(worldPos);

        _baseScale = Vector3.one;
        transform.localScale = Vector3.zero;   // ポップインアニメ用

        ApplyStyle();
        SetLabel(label);

        if (tooltipRoot != null) tooltipRoot.SetActive(false);

        // ポップイン
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.15f)
                 .setEase(LeanTweenType.easeOutBack)
                 .setOnComplete(() =>
                    LeanTween.scale(gameObject, Vector3.one, 0.1f));
    }

    // ── 毎フレーム位置更新 ────────────────────────────────────

    public void UpdatePosition(MapManager manager)
    {
        _rect.anchoredPosition = manager.UVToMapPos(UV);

        // ホバー時に軽くボブ
        if (_isHovered)
        {
            _bobTime += Time.deltaTime * 3f;
            _rect.anchoredPosition += Vector2.up * Mathf.Sin(_bobTime) * 3f;
        }
    }

    public void SetVisible(bool visible) => gameObject.SetActive(visible);

    // ── ラベル / タイプ変更 ───────────────────────────────────

    public void SetLabel(string label)
    {
        Label = label;
        if (labelText  != null) labelText.text  = label;
        if (tooltipText != null) tooltipText.text = label;
    }

    public void SetType(MapIconType type)
    {
        IconType = type;
        ApplyStyle();
    }

    // ── イベントハンドラ ─────────────────────────────────────

    public void OnPointerEnter(PointerEventData _)
    {
        _isHovered = true;
        _bobTime   = 0f;
        if (tooltipRoot != null) tooltipRoot.SetActive(true);
        LeanTween.scale(gameObject, Vector3.one * 1.25f, 0.12f).setEase(LeanTweenType.easeOutBack);
    }

    public void OnPointerExit(PointerEventData _)
    {
        _isHovered = false;
        if (tooltipRoot != null) tooltipRoot.SetActive(false);
        _rect.anchoredPosition = _manager.UVToMapPos(UV); // ボブリセット
        LeanTween.scale(gameObject, Vector3.one, 0.12f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 右クリック → 削除確認
            MapEvents.OnIconRightClick?.Invoke(this);
        }
        else
        {
            // 左クリック → マップを該当位置にフォーカス
            MapEvents.OnIconClick?.Invoke(this);
        }
    }

    // ── 内部ヘルパー ─────────────────────────────────────────

    private void ApplyStyle()
    {
        if (iconImage == null || iconConfig == null) return;

        var entry = iconConfig.GetEntry(IconType);
        if (entry == null) return;

        iconImage.sprite = entry.sprite;
        iconImage.color  = entry.color;
    }
}

/// <summary>
/// アイコン追加・クリックイベント
/// </summary>
public static partial class MapEvents
{
    public static System.Action<MapIcon> OnIconClick;
    public static System.Action<MapIcon> OnIconRightClick;
}
