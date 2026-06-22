using UnityEngine;

namespace chest
{
    /// <summary>
    /// 箱と蓋のレンダラーに同じマテリアルを反映する(エディタ編集時に自動同期)。
    /// </summary>
    public class MaterialSet : MonoBehaviour
    {
        [SerializeField] private Material material;

        [Header("箱の部分")]
        [SerializeField] private Renderer boxRenderer;

        [Header("蓋の部分")]
        [SerializeField] private Renderer lidRenderer;

        private void OnValidate()
        {
            if (material == null) return;

            if (boxRenderer != null)
            {
                var mats = boxRenderer.sharedMaterials;
                if (mats.Length > 1)
                {
                    mats[1] = material;
                    boxRenderer.sharedMaterials = mats;
                }
            }

            if (lidRenderer != null)
            {
                var mats = lidRenderer.sharedMaterials;
                if (mats.Length > 0)
                {
                    mats[0] = material;
                    lidRenderer.sharedMaterials = mats;
                }
            }
        }
    }

}
