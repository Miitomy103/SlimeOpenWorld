using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// カメラを管理するクラス
/// </summary>
public class CameraManager : MonoBehaviour
{
    static CameraManager instance;
    public static CameraManager Instance => instance;

    [SerializeField] CinemachineCamera virtualCamera;

    //Follow
    [SerializeField] CinemachineFollow follow;

    [SerializeField,Range(followMinOffset,followMaxOffset)] float followOffset = 7.5f;
    [SerializeField] float scrollSensitivity = 12f;
    [SerializeField] float scrollDamping = 8f;
    const float followMaxOffset = 26f;
    const float followMinOffset = 5f;
    const float scrollStopThreshold = 0.01f;

    float followOffsetVelocity;

    private void Awake()
    {
        if(instance!=null)Debug.LogWarning("Multiple CameraManager instances detected!");
        instance = this;

        var f = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        follow = f as CinemachineFollow;
        SetOffset(followOffset);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            SetOffset(followOffset);
        }
    }
#endif

    private void Update()
    {
        // マウスホイールでカメラの距離を調整
        if (InputData.Instance != null && InputData.Instance.IsUsingUI)
        {
            followOffsetVelocity = 0f;
            return;
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0f)
        {
            followOffsetVelocity -= scrollDelta * scrollSensitivity;
        }

        if (Mathf.Abs(followOffsetVelocity) <= scrollStopThreshold)
        {
            followOffsetVelocity = 0f;
            return;
        }

        SetOffset(followOffset + followOffsetVelocity * Time.deltaTime);

        if ((followOffset <= followMinOffset && followOffsetVelocity < 0f) ||
            (followOffset >= followMaxOffset && followOffsetVelocity > 0f))
        {
            followOffsetVelocity = 0f;
            return;
        }

        followOffsetVelocity = Mathf.Lerp(
            followOffsetVelocity,
            0f,
            1f - Mathf.Exp(-scrollDamping * Time.deltaTime));
    }

    public void SetFollow(Transform target)
    {
        virtualCamera.Follow = target;
    }

    public void SetCinemachine(CinemachineCamera cinemachineCamera)
    {
        virtualCamera.gameObject.SetActive(false);
        Transform follow= virtualCamera.Follow;
        virtualCamera = cinemachineCamera;
        virtualCamera.gameObject.SetActive(true);
        SetFollow(follow);
    }

    public void SetOffset(float value)
    {
        if(value>followMaxOffset)
        {
            value = followMaxOffset;
        }
        else if (value < followMinOffset)
        {
            value = followMinOffset;
        }

        followOffset = value;
        follow.FollowOffset = new Vector3(value,value,-value);
    }
}
