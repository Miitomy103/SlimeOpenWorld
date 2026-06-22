using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowUI : MonoBehaviour
{
    static public FollowUI Instance { get; private set; }
    public Transform target;  // 追従する対象
    [SerializeField] Vector3 offset = Vector3.up * 2f;  // オフセット（頭上など）
    [SerializeField] RectTransform rectTransform;
    Camera mainCam;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if(rectTransform==null) rectTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;
    }
    public void UIUpdate(Transform target)
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
            rectTransform.gameObject.SetActive(false);
            return;
        }

        rectTransform.gameObject.SetActive(true);
        rectTransform.position = screenPos;
    }
}

