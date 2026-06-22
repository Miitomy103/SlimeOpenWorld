using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 対象の頭上に追従するHPバー。プール管理され、Updateで毎フレーム画面座標に追従させる。
/// </summary>
public class FollowHPVar : PoolObject,ISlider
{
    public Transform target;  // 追従する対象
    [SerializeField] Vector3 offset = Vector3.up * 2f;  // オフセット（頭上など）
    [SerializeField] RectTransform rectTransform;
    Camera mainCam;
    [SerializeField]float duration = 0.1f;

    [SerializeField] Slider hpSlider;

    void Start()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if(hpSlider == null) hpSlider = GetComponentInChildren<Slider>();
        mainCam = Camera.main;
    }
    public void Update()
    {
        if (target == null)
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }

        // ワールド座標 → スクリーン座標
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + offset);

        // 画面外に出たら非表示にするなどの処理も可
        if (screenPos.z < 0) // カメラの裏
        {
            return;
        }

        rectTransform.gameObject.SetActive(true);
        rectTransform.position = screenPos;
    }

    public void SetTarget(Transform newTarget)
    {
        if(mainCam == null) mainCam = Camera.main;

        target = newTarget;

        // ワールド座標 → スクリーン座標
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + offset);

        // 画面外に出たら非表示にするなどの処理も可
        if (screenPos.z < 0) // カメラの裏
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }

        rectTransform.gameObject.SetActive(true);
        rectTransform.position = screenPos;
    }

    public void SetValue(float value, float maxValue)
    {
        if(gameObject.activeInHierarchy == false) return;
        StartCoroutine(SliderSetValueSmooth(value, maxValue));
    }
    IEnumerator SliderSetValueSmooth(float value, float maxValue)
    {
        float elapsed = 0f;
        float startValue = hpSlider.value;
        float targetValue = value / maxValue;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            hpSlider.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            yield return null;
        }
        hpSlider.value = targetValue;
    }
}

