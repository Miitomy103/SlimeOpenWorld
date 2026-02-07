using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController instance;

    public static PlayerController Instance => instance;


    [SerializeField] HostBase hostBase;
    public HostBase HostBase=>hostBase;

    public Action<HostBase> onHostChange { get;set; }

    [SerializeField] PossessRange possessRange;

    int experience = 0;
    public int Experience => experience;
    int gold = 0;
    public int Gold => gold;

    private void Awake()
    {
        instance = this;
    }
    public void SetHost(HostBase newHost,HostBase agoHost)
    {
        if (hostBase != null) hostBase.EndHost();
        hostBase = newHost;
        hostBase.StartHost(agoHost);
        onHostChange?.Invoke(hostBase);
        possessRange.angleTransform = hostBase.transform;
        possessRange.positionTransform = hostBase.transform;

        CameraManager.Instance.SetFollow(hostBase.CameraTarget);
    }
    private void Start()
    {
        HostBase host = hostBase;
        hostBase = null;
        SetHost(host, null);
    }
    private void Update()
    {
        hostBase.UpdateHost();
    }
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Experience gained: {amount} (Total: {experience})");
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gold gained: {amount} (Total: {gold})");
    }
}
