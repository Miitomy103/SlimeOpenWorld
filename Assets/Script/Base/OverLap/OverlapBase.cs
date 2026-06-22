using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 派生先で選んだ範囲のオブジェクトを取得するクラス。
/// </summary>
public abstract class OverlapBase : MonoBehaviour
{
    public abstract T[] OverlapAll<T>(string layerName="Default");

    protected abstract void OnDrawGizmosSelected();
}
