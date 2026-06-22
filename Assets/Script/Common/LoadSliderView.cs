using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public sealed class LoadSliderView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image childImage;

    [Header("自動位置設定")]
    [SerializeField]bool autoFindChildImage = true;
    [SerializeField]Vector2 startPos;
    [SerializeField]Vector2 endPos;

    [Header("テスト用")]
    [SerializeField,Range(0, 1)] float testFillAmount = 0f;

    private void Awake()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }
    private void Start()
    {
        if (autoFindChildImage)
        {
            startPos=new Vector2(rectTransform.anchoredPosition.x, -rectTransform.rect.height);
            endPos = new Vector2(rectTransform.anchoredPosition.x, 0);
        }
    }
    private void Reset()
    {
        rectTransform = GetComponent<RectTransform>();
        childImage = GetComponentInChildren<Image>();
        startPos = new Vector2(rectTransform.anchoredPosition.x, -rectTransform.rect.height);
        endPos = new Vector2(rectTransform.anchoredPosition.x, 0);

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
            SetFillAmount(testFillAmount);
    }
#endif

    /// <summary>
    /// 0〜1の範囲でスライダーの進捗を設定します
    /// </summary>
    public void SetFillAmount(float amount)
    {
        amount = Mathf.Clamp01(amount);
        if (childImage != null)
        {
            float yPos = Mathf.Lerp(startPos.y, endPos.y, amount);
            childImage.rectTransform.anchoredPosition = new Vector2(childImage.rectTransform.anchoredPosition.x, yPos);
        }
    }
}
