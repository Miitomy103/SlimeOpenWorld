using UnityEngine;

/// <summary>
/// ミニマップ上でプレイヤーの位置と向きを示すアイコンを管理するクラス。
/// </summary>
public class PlayerMapObject : MonoBehaviour,IMapObject
{
    [SerializeField] RectTransform icon;
    [SerializeField] float iconAngleOffset;
    Transform playerTransform;
    Vector3 prevPosition;

    public void Disable()
    {
        if (icon != null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void Enable(Transform canvas)
    {
        if (icon == null)
        {
            Debug.LogWarning("PlayerMapObject mapIcon is not assigned.");
            return;
        }

        icon.gameObject.SetActive(true);
    }

    public void UpdateMapObject(Camera miniMapCamera, RectTransform mapBounds)
    {
        if (playerTransform == null || icon == null || miniMapCamera == null || mapBounds == null) return;

        Vector3 relative = playerTransform.position - miniMapCamera.transform.position;
        Quaternion camRotation = Quaternion.Euler(0, -miniMapCamera.transform.eulerAngles.y, 0);
        Vector3 rotatedRelative = camRotation * relative;

        float camHeight = miniMapCamera.orthographicSize * 2f;
        float camWidth = camHeight * miniMapCamera.aspect;

        float mapWidth = mapBounds.rect.width;
        float mapHeight = mapBounds.rect.height;

        float x = rotatedRelative.x / camWidth * mapWidth;
        float y = rotatedRelative.z / camHeight * mapHeight;

        Vector2 pos = new Vector2(x, y);
        Vector2 iconHalf = icon.rect.size * 0.5f;

        float minX = -mapWidth / 2 + iconHalf.x;
        float maxX = mapWidth / 2 - iconHalf.x;
        float minY = -mapHeight / 2 + iconHalf.y;
        float maxY = mapHeight / 2 - iconHalf.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        icon.anchoredPosition = pos;

        Vector3 delta = playerTransform.position - prevPosition;
        prevPosition = playerTransform.position;
        delta.y = 0f;

        if (delta.sqrMagnitude > 0.00001f)
        {
            Vector3 rotatedDelta = camRotation * delta.normalized;
            float angle = Mathf.Atan2(rotatedDelta.x, rotatedDelta.z) * Mathf.Rad2Deg;
            icon.localEulerAngles = new Vector3(0f, 0f, -angle + iconAngleOffset);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerController.Instance == null) return;

        if (PlayerController.Instance.HostBase != null)
        {
            SetPlayerTransform(PlayerController.Instance.HostBase.transform);
        }

        PlayerController.Instance.onHostChange += OnHostChanged;
    }

    private void OnDestroy()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.onHostChange -= OnHostChanged;
        }
    }

    private void OnHostChanged(HostBase newHost)
    {
        SetPlayerTransform(newHost != null ? newHost.transform : null);
    }

    private void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
        if (player != null) prevPosition = player.position;
    }
}
