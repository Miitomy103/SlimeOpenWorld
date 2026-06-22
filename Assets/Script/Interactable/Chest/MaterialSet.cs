using UnityEngine;

namespace chest
{
    using UnityEngine;

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
