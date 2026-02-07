using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public RectTransform mapBounds;

    public Camera miniMapCamera;

    public List<IMapObject> enemyList = new List<IMapObject>();
    void Update()
    {
        MapObjects();
    }
    void MapObjects()
    {
        foreach (IMapObject map in enemyList)
        {
            if (map!=null)
            {
                map.UpdateMapObject(miniMapCamera, mapBounds);
            }
        }
    }


    public void AddList(IMapObject mapObj)
    {
        mapObj.Enable(mapBounds);
        enemyList.Add(mapObj);
    }
    public void RemoveList(IMapObject mapObj)
    {
        mapObj.Disable();
        enemyList.Remove(mapObj);
    }
}
