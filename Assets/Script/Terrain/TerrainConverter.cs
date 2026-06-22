using UnityEngine;

/// <summary>
/// TerrainData上の木(TreeInstance)を、対応するPrefabとして実際のGameObjectに変換配置するエディタ用ツール。
/// </summary>
public class TerrainConverter : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] treePrefabs;

    [SerializeField] Transform parentTransform;

    public void ConvertTerrain()
    {
        if (!terrain) terrain = GetComponent<Terrain>();
        if (!terrain)
        {
            Debug.LogError("Terrainが設定されていません！");
            return;
        }

        var data = terrain.terrainData;
        var treeInstances = data.treeInstances;
        var prototypes = data.treePrototypes;

        if (prototypes == null || prototypes.Length == 0)
        {
            Debug.LogWarning("Terrainに登録されたTreePrototypeがありません。");
            return;
        }

        Debug.Log($"🌲 Terrainに登録された木の種類: {prototypes.Length} 種類");

        for (int i = 0; i < treeInstances.Length; i++)
        {
            var tree = treeInstances[i];
            int protoIndex = tree.prototypeIndex;

            if (protoIndex < 0 || protoIndex >= prototypes.Length)
            {
                Debug.LogWarning($"prototypeIndex {protoIndex} が範囲外です。");
                continue;
            }

            GameObject sourcePrefab = prototypes[protoIndex].prefab;
            if (sourcePrefab == null)
            {
                Debug.LogWarning($"TreePrototype[{protoIndex}] にPrefabが設定されていません。");
                continue;
            }

            // Terrain座標 → ワールド座標変換
            Vector3 worldPos = Vector3.Scale(tree.position, data.size) + terrain.transform.position;
            worldPos.y = terrain.SampleHeight(worldPos) + terrain.transform.position.y;

            // 生成
            GameObject newTree = Instantiate(sourcePrefab, worldPos, Quaternion.Euler(0, tree.rotation * Mathf.Rad2Deg, 0), parentTransform);

            // スケール反映
            newTree.transform.localScale = Vector3.one * tree.heightScale;

            // 名前をわかりやすく
            newTree.name = $"{sourcePrefab.name}_Instance_{i}";
        }

        Debug.Log($"✅ {treeInstances.Length} 本の木をPrefabとして生成しました。");
    }
}
