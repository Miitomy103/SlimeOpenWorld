using UnityEngine;
using Unity.Cinemachine;

public class DynamicOrbitBySize : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerController playerController;

    [System.Serializable]
    public struct OrbitSettings
    {
        public float height;
        public float radius;
    }

    [SerializeField] private OrbitSettings orbit;

    private CinemachineOrbitalFollow orbitalFollow;

    private void Awake()
    {
        if (cineCam == null)
            cineCam = GetComponent<CinemachineCamera>();

        orbitalFollow = cineCam.GetComponent<CinemachineOrbitalFollow>();
        if (orbitalFollow == null)
        {
            Debug.LogError("CinemachineCamera に CinemachineOrbitalFollow を追加してください");
            return;
        }

        if (playerController != null)
        {
            ChangeTarget(playerController.HostBase);
        }
        else if (target != null)
        {
            CameraSetting();
        }
    }

    public void ChangeTarget(HostBase hostBase)
    {
        if (hostBase == null) return;

        target = hostBase.transform;
        //cineCam.Target.TrackingTarget = hostBase.cameraFollow;
        //cineCam.Target.LookAtTarget = hostBase.cameraFollow;

        CameraSetting();
    }

    public void CameraSetting()
    {
        if (target == null || orbitalFollow == null) return;

        float size = target.localScale.y;
        float radius = Mathf.Lerp(3f, 8f, size * orbit.radius);
        float height = Mathf.Lerp(1f, 4f, size * orbit.height);

        // ThreeRing Orbit を調整
        var orbits = orbitalFollow.Orbits;
        orbits.Top.Height = height * 1.2f;
        orbits.Center.Height = height;
        orbits.Bottom.Height = height * 0.5f;

        orbits.Top.Radius = radius;
        orbits.Center.Radius = radius;
        orbits.Bottom.Radius = radius;

        orbitalFollow.Orbits = orbits;

        // TargetOffset はターゲット中心からのオフセット
        orbitalFollow.TargetOffset = Vector3.zero;
    }
}
