using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RayCamera : MonoBehaviour
{
    [Header("設定")]
    private Transform target=>PlayerController.Instance.HostBase.transform;  // プレイヤーをターゲットに
    [SerializeField] private float fadeAlpha = 0.2f;   // 半透明時のアルファ値
    [SerializeField] private float fadeSpeed = 5f;      // フェード速度
    [SerializeField] private LayerMask occlusionLayer;  // 対象レイヤー

    // 現在半透明にしているオブジェクトを記憶
    private Dictionary<Renderer, OriginalMaterialData> _fadedObjects = new();

    private struct OriginalMaterialData
    {
        public Color[] originalColors;
        public float[] originalAlpha;
        public RenderingMode[] originalModes;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 今フレームで検出したRenderer
        var currentHits = new HashSet<Renderer>();

        Vector3 dir = target.position - transform.position;
        float dist = dir.magnitude;

        // カメラ→プレイヤー間のすべてのヒットを取得
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, dist, occlusionLayer);

        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;

            currentHits.Add(rend);

            // 初めて検出 → 元のマテリアル情報を保存してTransparentに切り替え
            if (!_fadedObjects.ContainsKey(rend))
            {
                SaveAndSetTransparent(rend);
            }

            // アルファをfadeAlphaに近づける
            FadeRenderer(rend, fadeAlpha);
        }

        // 今フレームで検出されなかったオブジェクトを元に戻す
        var toRestore = new List<Renderer>();
        foreach (var kvp in _fadedObjects)
        {
            if (!currentHits.Contains(kvp.Key))
            {
                FadeRenderer(kvp.Key, 1f);

                // アルファが1に近づいたら完全に復元
                if (IsFullyOpaque(kvp.Key))
                    toRestore.Add(kvp.Key);
            }
        }

        foreach (var rend in toRestore)
        {
            RestoreRenderer(rend);
            _fadedObjects.Remove(rend);
        }
    }

    void FadeRenderer(Renderer rend, float targetAlpha)
    {
        foreach (var mat in rend.materials)
        {
            Color c = mat.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            mat.color = c;
        }
    }

    bool IsFullyOpaque(Renderer rend)
    {
        foreach (var mat in rend.materials)
            if (mat.color.a < 0.99f) return false;
        return true;
    }

    void SaveAndSetTransparent(Renderer rend)
    {
        var data = new OriginalMaterialData
        {
            originalColors = new Color[rend.materials.Length]
        };

        for (int i = 0; i < rend.materials.Length; i++)
        {
            data.originalColors[i] = rend.materials[i].color;
            SetMaterialTransparent(rend.materials[i]);
        }

        _fadedObjects[rend] = data;
    }

    void RestoreRenderer(Renderer rend)
    {
        if (!_fadedObjects.TryGetValue(rend, out var data)) return;

        for (int i = 0; i < rend.materials.Length; i++)
        {
            SetMaterialOpaque(rend.materials[i]);
            rend.materials[i].color = data.originalColors[i];
        }
    }

    // Built-in RP用: マテリアルをTransparentモードに切り替え
    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 2); // Fade
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Mode", 0);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
    }
}