using System;
using UnityEngine;

/// <summary>
/// アタッチされたGameObjectに永続的な一意のIDを自動付与するコンポーネント。
/// セーブデータとGameObjectの対応付けに使用する。
/// </summary>
public class IdDataBehaviour : MonoBehaviour, IUniqueID
{
    [SerializeField, HideInInspector] private string uniqueID;
    public string UniqueID =>uniqueID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
