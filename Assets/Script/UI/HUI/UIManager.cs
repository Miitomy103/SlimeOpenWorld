using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] HPVar hpVar;
    public HPVar HPVar => hpVar;

    [Header("Screen Arrow UI")]
    [SerializeField] ScreenArrowUI arrowPrefab;
    [SerializeField] Transform arrowContainer;

    private Dictionary<string, ScreenArrowUI> arrows = new Dictionary<string, ScreenArrowUI>();

    [SerializeField] GameObject mapParent;
    [SerializeField] GameObject menuParent;

    GameObject[] panels => new GameObject[] { mapParent, menuParent };
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        GameObject activePanel = PanelActive();
        if(activePanel==null)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                mapParent.SetActive(true);
                InputData.Instance.IsUsingUI = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuParent.SetActive(true);
                InputData.Instance.IsUsingUI = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            activePanel.SetActive(false);
            InputData.Instance.IsUsingUI = false;
        }
    }

    GameObject PanelActive()
    {
        foreach (var panel in panels)
        {
            if (panel.activeSelf) return panel;
        }
        return null;
    }

    /// <summary>
    /// 新しい矢印を作成してターゲットを設定
    /// </summary>
    /// <param name="key">矢印を識別するためのキー</param>
    /// <param name="target">追跡するターゲット</param>
    public ScreenArrowUI CreateArrow(string key, Transform target)
    {
        // 既に存在する場合は削除
        if (arrows.ContainsKey(key))
        {
            RemoveArrow(key);
        }

        // 新しい矢印を生成
        ScreenArrowUI newArrow = Instantiate(arrowPrefab, arrowContainer);
        newArrow.SetTarget(target);
        arrows[key] = newArrow;

        return newArrow;
    }

    /// <summary>
    /// 指定した矢印のターゲットを変更
    /// </summary>
    public void SetArrowTarget(string key, Transform target)
    {
        if (arrows.TryGetValue(key, out ScreenArrowUI arrow))
        {
            arrow.SetTarget(target);
        }
        else
        {
            Debug.LogWarning($"Arrow with key '{key}' not found.");
        }
    }

    /// <summary>
    /// 指定した矢印の表示/非表示を切り替え
    /// </summary>
    public void SetArrowActive(string key, bool active)
    {
        if (arrows.TryGetValue(key, out ScreenArrowUI arrow))
        {
            arrow.SetActive(active);
        }
        else
        {
            Debug.LogWarning($"Arrow with key '{key}' not found.");
        }
    }

    /// <summary>
    /// 指定した矢印を削除
    /// </summary>
    public void RemoveArrow(string key)
    {
        if (arrows.TryGetValue(key, out ScreenArrowUI arrow))
        {
            Destroy(arrow.gameObject);
            arrows.Remove(key);
        }
    }

    /// <summary>
    /// すべての矢印を削除
    /// </summary>
    public void RemoveAllArrows()
    {
        foreach (var arrow in arrows.Values)
        {
            if (arrow != null)
            {
                Destroy(arrow.gameObject);
            }
        }
        arrows.Clear();
    }

    /// <summary>
    /// 指定した矢印を取得
    /// </summary>
    public ScreenArrowUI GetArrow(string key)
    {
        arrows.TryGetValue(key, out ScreenArrowUI arrow);
        return arrow;
    }
}