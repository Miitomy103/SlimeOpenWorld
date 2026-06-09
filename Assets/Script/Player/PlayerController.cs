using System;
using UnityEngine;

/// <summary>
/// プレイヤー（ホスト）を管理するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    static PlayerController instance;
    public static PlayerController Instance => instance;

    [SerializeField] HostBase defaultHostPrefab;
    [SerializeField] Vector3 defaultPosition;
    [SerializeField] PossessRange possessRange;


    [SerializeField] HostBase hostBase;
    /// <summary>
    /// 現在乗っ取っているホスト。
    /// </summary>
    public HostBase HostBase => hostBase;
    public Action<HostBase> onHostChange { get; set; }

    int experience = 0;
    public int Experience => experience;
    int gold = 0;
    public int Gold => gold;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Initialize();
    }

    // SaveManagerのAwakeより後、かつLoadより前に呼ばれる想定
    public void Initialize()
    {
        // デフォルトホストをインスタンス化して設定
        HostBase host = Instantiate(defaultHostPrefab);
        SetHost(host, null);
        host.transform.position = defaultPosition;
    }

    private void Update()
    {
        hostBase?.UpdateHost();
    }

    /// <summary>
    /// ホストを切り替える。
    /// </summary>
    public void SetHost(HostBase newHost, HostBase agoHost)
    {
        if (hostBase != null) hostBase.EndHost();
        hostBase = newHost;
        hostBase.StartHost(agoHost);
        onHostChange?.Invoke(hostBase);
        possessRange.angleTransform = hostBase.transform;
        possessRange.positionTransform = hostBase.transform;
        CameraManager.Instance.SetFollow(hostBase.CameraTarget);
    }

    // SaveManagerから呼ばれる
    public void ApplySaveData(PlayerSaveData data)
    {
        if (data == null) { Debug.LogError("PlayerSaveData is null"); return; }

        // ホストの復元（SlimeHostはデフォルトのままにする等の判定）
        string typeName = data.hostName;
        HostBase prefab = Resources.Load<HostBase>("Hosts/" + typeName);
        if (prefab != null && prefab is not SlimeHost)
        {
            HostBase newHost = Instantiate(prefab);
            Debug.Log($"Loaded host: {(newHost == null)}");
            SetHost(newHost, hostBase);
        }
        else
        {
            HostBase newHost = Instantiate(defaultHostPrefab);

            SetHost(newHost, hostBase);
        }

        Initialize(); // デフォルトホストで初期化
        Debug.Log($"Host{hostBase==null}, Position{data.position}, Scene{data.sceneName}");
        hostBase.transform.position = data.position;

        // シーン遷移はSceneController経由で行う
        SceneController.Instance.LoadScene(data.sceneName, data.position);
    }

    //仮で追加した
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Experience gained: {amount} (Total: {experience})");
    }

    //仮で追加した
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gold gained: {amount} (Total: {gold})");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float lineLength = 2f;
        Gizmos.DrawLine(float.IsNaN(defaultPosition.x) ? transform.position : defaultPosition,
                        float.IsNaN(defaultPosition.x) ? transform.position + Vector3.forward * lineLength : defaultPosition + Vector3.forward * lineLength);
        Gizmos.DrawLine(float.IsNaN(defaultPosition.x) ? transform.position : defaultPosition,
                        float.IsNaN(defaultPosition.x) ? transform.position + Vector3.right * lineLength : defaultPosition + Vector3.right * lineLength);
    }
}