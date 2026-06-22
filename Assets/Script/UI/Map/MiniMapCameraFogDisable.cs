using UnityEngine;

/// <summary>
/// このカメラの描画中だけフォグを無効化する(ミニマップにフォグを映さないため)。
/// </summary>
public class MiniMapCameraFogDisable : MonoBehaviour
{
    bool prevFog;

    void OnPreRender()
    {
        prevFog = RenderSettings.fog;
        RenderSettings.fog = false;
    }

    void OnPostRender()
    {
        RenderSettings.fog = prevFog;
    }
}
