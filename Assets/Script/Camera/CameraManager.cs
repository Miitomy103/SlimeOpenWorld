using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager instance;
    public static CameraManager Instance => instance;

    [SerializeField] CinemachineCamera virtualCamera;

    //Forrow
    [SerializeField] CinemachineFollow follow;

    [SerializeField,Range(followMaxOffset,followMinOffset)] float followOffset = 7.5f;
    const float followMaxOffset = 13f;
    const float followMinOffset = 5f;

    private void Awake()
    {
        if(instance!=null)Debug.LogWarning("Multiple CameraManager instances detected!");
        instance = this;

        var f = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        follow = f as CinemachineFollow;
        SetOffset(followOffset);
    }
    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            SetOffset(followOffset);
        }
    }

    private void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            followOffset -= Input.mouseScrollDelta.y;
            SetOffset(followOffset);
        }

    }

    public void SetFollow(Transform target)
    {
        virtualCamera.Follow = target;
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
