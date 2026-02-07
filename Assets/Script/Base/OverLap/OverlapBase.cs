using Unity.VisualScripting;
using UnityEngine;

public abstract class OverlapBase : MonoBehaviour
{
    public abstract T[] OverlapAll<T>(string layerName="Default");

    protected abstract void OnDrawGizmosSelected();
}
