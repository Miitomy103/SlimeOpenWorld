using UnityEngine;

/// <summary>
/// CullingGroupを使い、対象との距離に応じて段階的に処理を間引くLODコンポーネント。
/// </summary>
public class CullingGroupDistanceLOD : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] objects;
    [SerializeField] float[] distanceBands = { 10f, 30f, 60f };

    CullingGroup group;
    BoundingSphere[] spheres;

    void Start()
    {
        int count = objects.Length;

        spheres = new BoundingSphere[count];
        for (int i = 0; i < count; i++)
        {
            spheres[i] = new BoundingSphere(objects[i].position, 1f);
        }

        group = new CullingGroup();
        group.SetBoundingSpheres(spheres);
        group.SetBoundingSphereCount(count);

        group.SetDistanceReferencePoint(player);
        //TODO: 配列の最後に Mathf.Infinity を追加する必要がある

        group.onStateChanged = OnChange;
    }

    void Update()
    {
        // 球の位置は毎フレーム更新する必要がある
        for (int i = 0; i < objects.Length; i++)
        {
            spheres[i].position = objects[i].position;
        }
    }

    void OnChange(CullingGroupEvent evt)
    {
        int index = evt.index;

        int newLevel = evt.currentDistance;
        int oldLevel = evt.previousDistance;

        Debug.Log($"Object {index} : {oldLevel} → {newLevel}");

        // LOD処理
        switch (newLevel)
        {
            case 0: SetNear(objects[index]); break;
            case 1: SetMid(objects[index]); break;
            case 2: SetFar(objects[index]); break;
            case 3: SetVeryFar(objects[index]); break;
        }
    }

    void SetNear(Transform obj)
    {
        // フル処理
        obj.gameObject.SetActive(true);
    }

    void SetMid(Transform obj)
    {
        // アニメだけ止めるなど
    }

    void SetFar(Transform obj)
    {
        // AI停止・Update停止など
    }

    void SetVeryFar(Transform obj)
    {
        // 完全に非アクティブ化など
        obj.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        group?.Dispose();
    }
}
