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

    [SerializeField,Range(followMaxOffset,followMinOffset)] float followOffset = 7.5f;
    const float followMaxOffset = 26f;
    const float followMinOffset = 5f;

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
        if (Input.mouseScrollDelta.y != 0||!InputData.Instance.IsUsingUI)
        {
            followOffset -= Input.mouseScrollDelta.y;
            SetOffset(followOffset);
        }

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

        follow.FollowOffset = new Vector3(value,value,-value);
    }
}
