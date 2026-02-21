using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// マップ画面全体の UI を制御するクラス
/// ・マップパネルの開閉
/// ・アイコン種別選択パレット
/// ・ダブルクリックでのアイコン追加フロー
/// ・右クリックでのアイコン削除確認
/// </summary>
public class MapUI : MonoBehaviour
{
    [Header("=== パネル参照 ===")]
    public GameObject  mapPanel;           // マップ全体パネル
    public MapManager  mapManager;

    [Header("=== アイコン追加 UI ===")]
    public GameObject       iconPalette;        // アイコン種別選択パレット
    public Transform        paletteButtonRoot;  // ボタンの親
    public MapIconTypeButton paletteButtonPrefab;

    [Header("=== ラベル入力 ===")]
    public GameObject  labelInputPanel;
    public TMP_InputField labelInputField;
    public Button      labelConfirmBtn;
    public Button      labelCancelBtn;

    [Header("=== 削除確認 ===")]
    public GameObject  deleteConfirmPanel;
    public TextMeshProUGUI deleteConfirmText;
    public Button      deleteYesBtn;
    public Button      deleteNoBtn;

    [Header("=== ズームボタン ===")]
    public Button zoomInBtn;
    public Button zoomOutBtn;
    public Button zoomResetBtn;
    public Button followPlayerBtn;

    [Header("=== 設定 ===")]
    public KeyCode toggleKey = KeyCode.M;
    public float   zoomButtonStep = 0.5f;

    // ── 状態 ─────────────────────────────────────────────────────
    private bool         _isOpen;
    private Vector2      _pendingWorldPos;
    private MapIconType  _pendingType;
    private MapIcon      _pendingDeleteIcon;

    // ── Unity ────────────────────────────────────────────────────
    void Awake()
    {
        BuildPalette();
        BindButtons();
        SubscribeEvents();

        // 初期状態は閉じる
        mapPanel.SetActive(false);
        iconPalette.SetActive(false);
        labelInputPanel.SetActive(false);
        deleteConfirmPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleMap();
    }

    void OnDestroy()
    {
        MapEvents.OnMapDoubleClick  -= OnMapDoubleClick;
        MapEvents.OnIconClick       -= OnIconClick;
        MapEvents.OnIconRightClick  -= OnIconRightClick;
    }

    // ── 公開 API ─────────────────────────────────────────────────

    public void ToggleMap()
    {
        _isOpen = !_isOpen;
        // フェードアニメ (LeanTween)
        if (_isOpen)
        {
            mapPanel.SetActive(true);
            CanvasGroup cg = mapPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                LeanTween.alphaCanvas(cg, 1f, 0.25f);
            }
        }
        else
        {
            CanvasGroup cg = mapPanel.GetComponent<CanvasGroup>();
            if (cg != null)
                LeanTween.alphaCanvas(cg, 0f, 0.2f).setOnComplete(() => mapPanel.SetActive(false));
            else
                mapPanel.SetActive(false);
        }
    }

    // ── アイコン追加フロー ────────────────────────────────────────

    private void OnMapDoubleClick(Vector2 worldPos, Vector2 uv)
    {
        _pendingWorldPos = worldPos;
        ShowIconPalette(worldPos);
    }

    private void ShowIconPalette(Vector2 worldPos)
    {
        iconPalette.SetActive(true);
        // パレットをクリック位置付近に移動（任意）
    }

    private void OnPaletteTypeSelected(MapIconType type)
    {
        _pendingType = type;
        iconPalette.SetActive(false);
        ShowLabelInput();
    }

    private void ShowLabelInput()
    {
        labelInputPanel.SetActive(true);
        labelInputField.text = string.Empty;
        labelInputField.Select();
        labelInputField.ActivateInputField();
    }

    private void OnLabelConfirm()
    {
        string label = labelInputField.text.Trim();
        mapManager.AddIcon(_pendingWorldPos, _pendingType, label);
        labelInputPanel.SetActive(false);
    }

    private void OnLabelCancel()
    {
        labelInputPanel.SetActive(false);
    }

    // ── アイコンクリック処理 ──────────────────────────────────────

    private void OnIconClick(MapIcon icon)
    {
        // 左クリック: マップをそのアイコン中心にフォーカス (追従解除)
        // 必要に応じて詳細ウィンドウ表示などを追加
        Debug.Log($"[MapUI] アイコンクリック: {icon.Label} ({icon.IconType}) at {icon.WorldPosition}");
    }

    private void OnIconRightClick(MapIcon icon)
    {
        _pendingDeleteIcon = icon;
        deleteConfirmText.text = $"「{icon.Label}」を削除しますか？";
        deleteConfirmPanel.SetActive(true);
    }

    private void OnDeleteConfirm()
    {
        if (_pendingDeleteIcon != null)
            mapManager.RemoveIcon(_pendingDeleteIcon);
        _pendingDeleteIcon = null;
        deleteConfirmPanel.SetActive(false);
    }

    private void OnDeleteCancel()
    {
        _pendingDeleteIcon = null;
        deleteConfirmPanel.SetActive(false);
    }

    // ── ボタンバインド ────────────────────────────────────────────

    private void BindButtons()
    {
        zoomInBtn?.onClick.AddListener(()  => mapManager.SetZoom(
            Mathf.Min(mapManager.zoomMax, GetCurrentZoom() + zoomButtonStep)));
        zoomOutBtn?.onClick.AddListener(() => mapManager.SetZoom(
            Mathf.Max(mapManager.zoomMin, GetCurrentZoom() - zoomButtonStep)));
        zoomResetBtn?.onClick.AddListener(() => mapManager.SetZoom(1f));
        followPlayerBtn?.onClick.AddListener(() => mapManager.ResumeFollowPlayer());

        labelConfirmBtn?.onClick.AddListener(OnLabelConfirm);
        labelCancelBtn?.onClick.AddListener(OnLabelCancel);
        deleteYesBtn?.onClick.AddListener(OnDeleteConfirm);
        deleteNoBtn?.onClick.AddListener(OnDeleteCancel);
    }

    private void SubscribeEvents()
    {
        MapEvents.OnMapDoubleClick += OnMapDoubleClick;
        MapEvents.OnIconClick      += OnIconClick;
        MapEvents.OnIconRightClick += OnIconRightClick;
    }

    // ── パレット生成 ──────────────────────────────────────────────

    private void BuildPalette()
    {
        if (paletteButtonPrefab == null || paletteButtonRoot == null) return;

        foreach (MapIconType type in System.Enum.GetValues(typeof(MapIconType)))
        {
            var btn = Instantiate(paletteButtonPrefab, paletteButtonRoot);
            btn.Setup(type, OnPaletteTypeSelected);
        }
    }

    private float GetCurrentZoom() => 1f; // 必要に応じて MapManager からズーム値を取得
}

/// <summary>
/// アイコン種別選択パレットの1ボタン
/// </summary>
public class MapIconTypeButton : MonoBehaviour
{
    public Image           iconImage;
    public TextMeshProUGUI label;
    private Button         _btn;
    private MapIconType    _type;
    private System.Action<MapIconType> _callback;

    public void Setup(MapIconType type, System.Action<MapIconType> callback)
    {
        _type     = type;
        _callback = callback;
        _btn      = GetComponent<Button>();
        if (label != null) label.text = type.ToString();
        _btn.onClick.AddListener(() => _callback?.Invoke(_type));
    }
}
