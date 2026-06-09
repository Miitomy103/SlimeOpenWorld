using UnityEngine;

/// <summary>
/// 乗っ取り可能なオブジェクトの基底クラス乗っ取り後はHostBaseを生成する
/// </summary>
public abstract class PossessBase : MonoBehaviour, IPossess
{
    public Transform Transform => transform;

    /// <summary>
    /// 乗っ取り可能かどうか。基本的にはtrueを返す。
    /// </summary>
    public virtual bool CanPossess => true;

    [SerializeField]string possessId = "DefaultID";
    /// <summary>
    /// クエストやUIなどで識別するためのID。
    /// </summary>
    public virtual string PossessId =>possessId;

    [SerializeField] HostBase hostPrefab;
    public virtual HostBase GetHost()
    {
        HostBase hostInstance = Instantiate(hostPrefab, Transform.position, Transform.rotation);
        gameObject.SetActive(false);
        return hostInstance;
    }
}
