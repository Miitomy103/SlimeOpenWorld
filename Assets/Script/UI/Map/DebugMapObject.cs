using UnityEngine;

public class DebugMapObject : MonoBehaviour
{
    [SerializeField] EnemyMapObject[] enemyMapObject;

    [SerializeField] MapObject mapObject;

    private void Start()
    {
        foreach (var enemy in enemyMapObject)
        {
            mapObject.AddList(enemy);
        }
    }
}
