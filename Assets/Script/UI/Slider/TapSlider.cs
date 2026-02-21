using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TapSlider : MonoBehaviour
{
    [SerializeField] Image fillRect;
    [SerializeField] UnityEvent<float> onValueChanged;
    [SerializeField] UnityEvent<float> onBeginDrag;
    [SerializeField] UnityEvent<float> onEndDrag;
    [SerializeField] ClickEvent clickEvent;
    [SerializeField] string key;
    public string Key => key;

    bool onMouseDown;

    private void Awake()
    {
        if (fillRect==null)
        {
            fillRect=GetComponent<Image>();
        }
        if (clickEvent != null)
        {
            clickEvent.onPointerDown += OnPointerDown;
        }
    }
    public void SetFillAmount(float value)
    {
        fillRect.fillAmount = value;
    }
    private void OnPointerDown()
    {
        onMouseDown = true;
        onBeginDrag?.Invoke(fillRect.fillAmount);
    }

    private void Update()
    {
        if (!onMouseDown) return;

        RectTransform rt = fillRect.rectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        // 左端を 0 にする
        float x = localPos.x + rt.rect.width * rt.pivot.x;

        // 0〜1 に正規化
        float value = Mathf.Clamp01(x / rt.rect.width);

        // Fill 更新
        SetFillAmount(value);

        // イベント通知
        onValueChanged?.Invoke(value);

        if(Input.GetMouseButtonUp(0))
        {
            onMouseDown = false;
            onEndDrag?.Invoke(value);
        }
    }

}

