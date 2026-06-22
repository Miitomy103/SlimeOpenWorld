using UnityEngine;

/// <summary>
/// オブジェクトと数値を1組にして保持する汎用データクラス。
/// </summary>
[System.Serializable]
public class ObjectFloatData<T>
{
    [SerializeField] T obj;
    public T Obj => obj;
    [SerializeField] float value;
    public float Value => value;

    ObjectFloatData(T obj, float value)
    {
        this.obj = obj;
        this.value = value;
    }
}
