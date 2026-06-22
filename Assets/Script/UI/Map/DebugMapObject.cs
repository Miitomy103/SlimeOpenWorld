using UnityEngine;

/// <summary>
/// デバッグ用: シーン上の敵アイコンをまとめてMapObjectへ登録するハーネス。
/// </summary>
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
