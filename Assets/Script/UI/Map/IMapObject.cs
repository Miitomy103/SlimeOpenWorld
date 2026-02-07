using UnityEngine;

public interface IMapObject
{
    void Enable(Transform canvas);
    void Disable();
    public void UpdateMapObject(Camera miniMapCamera,RectTransform mapBounds);
}
