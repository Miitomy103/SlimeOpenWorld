using UnityEngine;

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
